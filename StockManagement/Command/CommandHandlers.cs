using StockManagement.Aggregates;
using StockManagement.Events;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Command

{
    public class CommandHandler
        : ICommandHandler<CreateStockCommand>, ICommandHandler<UpdateStockCommand>, ICommandHandler<DeleteStockCommand>,
         ICommandHandler<CreateSupplierCommand>, ICommandHandler<UpdateSupplierCommand>, ICommandHandler<DeleteSupplierCommand>,
        ICommandHandler<AddStockToSupplierCommand>, ICommandHandler<RemoveStockFromSupplierCommand>
    {
        private readonly StockContext db;
        private readonly IEventStore eventstore;
        private readonly IPublisher publisher;
        public CommandHandler(StockContext db, IEventStore eventstore, IPublisher publisher)
        {
            this.db = db;
            this.eventstore = eventstore;
            this.publisher = publisher;
        }

        public async void Handle(CreateStockCommand command)
        {

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
                    
            var stockAggr = await eventstore.GetStockAggregateByID(command.UpdateStock.Id);
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

            DeleteStockCommand cmd = new DeleteStockCommand { Id = command.Id };
            var stockAggr = await eventstore.GetStockAggregateByID(cmd.Id);
            stockAggr.Delete(cmd);
            eventstore.Save(stockAggr);
            foreach (var pEvent in stockAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }

        public async void Handle(DeleteSupplierCommand command)
        {

            DeleteSupplierCommand cmd = new DeleteSupplierCommand { SupplierId = command.SupplierId };
            var supplAggr = await eventstore.GetSupplierAggregateByID(cmd.SupplierId);
            supplAggr.Delete(cmd);
            eventstore.Save(supplAggr);
            foreach (var pEvent in supplAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }

        public async void Handle(CreateSupplierCommand command)
        {
            command.CreateSupplier.Id = Guid.NewGuid();
            CreateSupplierCommand cmd = new CreateSupplierCommand { CreateSupplier = command.CreateSupplier };
            var supplAggr = new SupplierAggregate(cmd.CreateSupplier.Id);
            supplAggr.Create(command);
            eventstore.Save(supplAggr);
            foreach (var pEvent in supplAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }

        public async void Handle(UpdateSupplierCommand command)
        {

            var supplierAggr = await eventstore.GetSupplierAggregateByID(command.UpdateSupplier.Id);
            supplierAggr.Update(command);
            eventstore.Save(supplierAggr);
            foreach (var pEvent in supplierAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }

        public async void Handle(AddStockToSupplierCommand command)
        {

            var supplierAggr = await eventstore.GetSupplierAggregateByID(command.SupplierId);
            supplierAggr.AddStockToSupplier(command);
            eventstore.Save(supplierAggr);
            foreach (var pEvent in supplierAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }

        public async void Handle(RemoveStockFromSupplierCommand command)
        {

            var supplierAggr = await eventstore.GetSupplierAggregateByID(command.SupplierId);
            supplierAggr.RemoveStockFromSupplier(command);
            eventstore.Save(supplierAggr);
            foreach (var pEvent in supplierAggr.PendingEvents)
            {
                await publisher.Publish(GetPublishModelString(pEvent));
            }

        }
    }

}
