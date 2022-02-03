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
        public StockCommandHandler(StockContext db, IEventStore eventstore)
        {
            this.db = db;
            this.eventstore = eventstore;
        }

        public void Handle(CreateStockCommand command)
        {
            db.Stocks.Add(command.CreateStock);
            db.SaveChanges();
            var stockAggr = new StockAggregate(command.CreateStock.Id);
            stockAggr.Create(command);
            eventstore.Save(stockAggr);
        }
        public async void Handle(UpdateStockCommand command)
        {
            db.Stocks.Update(command.UpdateStock);
            db.SaveChanges();                       
            var stockAggr = await eventstore.GetAggregateByID(command.UpdateStock.Id);
            stockAggr.Update(command);
            eventstore.Save(stockAggr);
        }

        public async void Handle(DeleteStockCommand command)
        {
            var dbEntity = db.Stocks.FirstOrDefault(x => x.Id == command.Id);
            if (dbEntity !=null)
            {
                dbEntity.Status = "closed";
                db.Stocks.Update(dbEntity);
                db.SaveChanges();
                DeleteStockCommand cmd = new DeleteStockCommand { Id = dbEntity.Id};
                var stockAggr = await eventstore.GetAggregateByID(cmd.Id);
                stockAggr.Delete(cmd);
                eventstore.Save(stockAggr);
            }
            
        }
    }

}
