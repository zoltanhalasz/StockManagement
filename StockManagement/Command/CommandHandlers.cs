using StockManagement.Aggregates;
using StockManagement.Events;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Command

{
    public class StockCommandHandler
        : ICommandHandler<CreateStockCommand>, ICommandHandler<UpdateStockCommand>, ICommandHandler<DeleteStockCommand>
    {
        private readonly StockContext db;
        private readonly IEventStore eventstore;
        private readonly IPublisher publisher;
        public StockCommandHandler(StockContext db, IEventStore eventstore, IPublisher publisher)
        {
            this.db = db;
            this.eventstore = eventstore;
            this.publisher = publisher;
        }

        public async void Handle(CreateStockCommand command)
        {
            //db.Stocks.Add(command.CreateStock);
            //db.SaveChanges();
            command.CreateStock.Id = Guid.NewGuid();
            var stockAggr = new StockAggregate(command.CreateStock.Id);
            stockAggr.Create(command);
            eventstore.Save(stockAggr);
            foreach (var pEvent in stockAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }
        public async void Handle(UpdateStockCommand command)
        {
            //db.Stocks.Update(command.UpdateStock);
            //db.SaveChanges();                       
            var stockAggr = await eventstore.GetAggregateByID(command.UpdateStock.Id);
            stockAggr.Update(command);
            eventstore.Save(stockAggr);
            foreach (var pEvent in stockAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }
            
        }

        private string GetPublishModelString(IEvent myevent)
        {
            var publishModelString = Newtonsoft.Json.JsonConvert.SerializeObject(
                                    new PublishModel
                                    {
                                        Payload = Newtonsoft.Json.JsonConvert.SerializeObject(myevent),
                                        EventType = myevent.GetType().ToString()
                                    }
                                    );
            return publishModelString;
        }
        public async void Handle(DeleteStockCommand command)
        {
            //var dbEntity = db.Stocks.FirstOrDefault(x => x.Id == command.Id);
            //if (dbEntity !=null)
            //{
            //    dbEntity.Status = "closed";
            //    db.Stocks.Update(dbEntity);
            //    db.SaveChanges();
               

            //}
            DeleteStockCommand cmd = new DeleteStockCommand { Id = command.Id };
            var stockAggr = await eventstore.GetAggregateByID(cmd.Id);
            stockAggr.Delete(cmd);
            eventstore.Save(stockAggr);
            foreach (var pEvent in stockAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }
    }

}
