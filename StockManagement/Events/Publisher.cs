using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace StockManagement.Events
{

    public class PublishModel
    {
        public string Payload { get; set; }
        public string EventType{ get; set; }
    }

    public interface IPublisher
    {
        Task Publish(string message);
        
    }
    public class Publisher : IPublisher
    {
        static string connectionString = "Endpoint=sb://zoltan-halasz-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HNMMcwc60PduObfPLPZ+750oPQfzXdJmVK+4mgnrDSk=";

        // name of your Service Bus topic
        static string topicName = "mytopic";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the sender used to publish messages to the topic
        static ServiceBusSender sender;

        public async Task Publish(string message)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(topicName);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
            {
                
                throw new Exception($"The message {message} is too large to fit in the batch.");
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
            
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

    }
}
