using StockManagement.Command;
using StockManagement.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Aggregates
{

    public class SupplierAggregate
    {
        public List<IEvent> PendingEvents { get; set; } = new List<IEvent>();
        public List<IEvent> AllEvents { get; set; } = new List<IEvent>();
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }


        public SupplierAggregate(Guid id)
        {
            this.Id = id;
        }

        public SupplierAggregate()
        {
            
        }
        public void Create(CreateSupplierCommand command)
        {
            var @event = new SupplierCreated { Supplier = command.CreateSupplier};            
            PendingEvents.Add(@event);
            AllEvents.Add(@event);            
        }

        public void Update(UpdateSupplierCommand command)
        {
            var @event = new SupplierUpdated { Supplier= command.UpdateSupplier};

            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }

        public void Delete(DeleteSupplierCommand command)
        {
            var @event = new SupplierDeleted { SupplierId = command.SupplierId};
            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }

        public void AddStockToSupplier(AddStockToSupplierCommand command)
        {
            var @event = new StockAddedToSupplier { SupplierId = command.SupplierId, StockId = command.StockId };
            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }


        public void RemoveStockFromSupplier(RemoveStockFromSupplierCommand command)
        {
            var @event = new StockRemovedFromSupplier{ SupplierId = command.SupplierId, StockId = command.StockId };
            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }



        private void HandleEvent(IEvent supplEvent)
        {
            if (supplEvent is SupplierCreated)
            {
                Handle(supplEvent as SupplierCreated);
            }
            if (supplEvent is SupplierUpdated)
            {
                Handle(supplEvent as SupplierUpdated);
            }
            if (supplEvent is SupplierDeleted)
            {
                Handle(supplEvent as SupplierDeleted);
            }
            if (supplEvent is StockAddedToSupplier)
            {
                Handle(supplEvent as StockAddedToSupplier);
            }
            if (supplEvent is StockRemovedFromSupplier)
            {
                Handle(supplEvent as StockRemovedFromSupplier);
            }
        }


        private void Handle(SupplierCreated @event)
        {
            Id = @event.Supplier.Id;
            Name = @event.Supplier.Name;
            Address= @event.Supplier.Address;
        }

        private void Handle(SupplierUpdated @event)
        {
            Id = @event.Supplier.Id;
            Name = @event.Supplier.Name;
            Address = @event.Supplier.Address;
        }

        private void Handle(SupplierDeleted @event)
        {
            
            Name = Name + " --deleted";
            Address = Address + " --deleted";
        }
        private void Handle(StockAddedToSupplier @event)
        {

        }

        private void Handle(StockRemovedFromSupplier @event)
        {
        }
        public void ReconstituteFromHistory(IEnumerable<IEvent> supplEvents)
        {
            foreach (var supplEvent in supplEvents)
            {
                HandleEvent(supplEvent);
            }
        }
    }
}
