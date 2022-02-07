using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StockManagement.Models;
using WebJobStocks.Models;

namespace WebJobStocks
{
    class Program
    {



        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // add necessary services

            services.AddDbContext<StockContext>(options => options.UseSqlServer("Server=tcp:zozoserver.database.windows.net,1433;Initial Catalog=stockdb;Persist Security Info=False;User ID=zoltanhalasz;Password=Nyar18Zozo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            services.AddScoped<IEventHandler, Models.EventHandler>();
            services.AddScoped<IASBMessageProcessor, ASBMessageProcessor>();            
            // build the pipeline
            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<IASBMessageProcessor>();
            await service.Process();
        }
    }
    public interface IASBMessageProcessor
    {
        Task Process();
    }

    public class ASBMessageProcessor: IASBMessageProcessor
    {
        // connection string to your Service Bus namespace
        string connectionString = "Endpoint=sb://zoltan-halasz-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HNMMcwc60PduObfPLPZ+750oPQfzXdJmVK+4mgnrDSk=";

        // name of the Service Bus topic
        string topicName = "mytopic";

        // name of the subscription to the topic
        string subscriptionName = "S1";

        // the client that owns the connection and can be used to create senders and receivers
        ServiceBusClient client;

        // the processor that reads and processes messages from the subscription
        ServiceBusProcessor processor;

        private readonly IEventHandler eventHandler;
        public ASBMessageProcessor(IEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
        }




        // handle received messages
         async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");

            var type = typeof(PublishModel);

            var publishModel = JsonConvert.DeserializeObject(body, type) as PublishModel;
            int i = publishModel.EventType.LastIndexOf('.');
            string newType = "WebJobStocks.Models." + publishModel.EventType.Substring(i + 1);
            type = Type.GetType(newType);
            var eventDeserialized = JsonConvert.DeserializeObject(publishModel.Payload, type) as IEvent;
            Console.WriteLine(eventDeserialized as IEvent);

            //var options = new DbContextOptionsBuilder<StockContext>();
            //options.UseSqlServer("Server=tcp:zozoserver.database.windows.net,1433;Initial Catalog=stockdb;Persist Security Info=False;User ID=zoltanhalasz;Password=Nyar18Zozo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            //var db = new StockContext(options.Options);

            await eventHandler.Handle(eventDeserialized);
            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Process()
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);

            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();
                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }

}
