using StockManagement.Command;
using StockManagement.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Aggregates
{

    public class StockAggregate
    {
        public List<IEvent> PendingEvents { get; set; } = new List<IEvent>();
        public List<IEvent> AllEvents { get; set; } = new List<IEvent>();
        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public string Item { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Batch { get; set; }
        public string CountryOfOrigin { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Quantity { get; set; }


        public StockAggregate(Guid id)
        {
            this.Id = id;
        }

        public StockAggregate()
        {
            
        }
        public void Create(CreateStockCommand command)
        {
            var @event = new StockCreated { Stock = command.CreateStock };            
            PendingEvents.Add(@event);
            AllEvents.Add(@event);            
        }

        public void Update(UpdateStockCommand command)
        {
            var @event = new StockUpdated { Stock = command.UpdateStock};

            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }

        public void Delete(DeleteStockCommand command)
        {
            var @event = new StockDeleted { Id = command.Id};
            PendingEvents.Add(@event);
            AllEvents.Add(@event);
        }

        private void HandleEvent(IEvent stockEvent)
        {
            if (stockEvent is StockCreated)
            {
                Handle(stockEvent as StockCreated);
            }
            if (stockEvent is StockUpdated)
            {
                Handle(stockEvent as StockUpdated);
            }
            if (stockEvent is StockDeleted)
            {
                Handle(stockEvent as StockDeleted);
            }
        }


        private void Handle(StockCreated @event)
        {
            Id = @event.Stock.Id;
            LicensePlate = @event.Stock.LicensePlate;
            Item = @event.Stock.Item;
            Location = @event.Stock.Location;
            Status = @event.Stock.Status;
            Batch = @event.Stock.Batch;
            CountryOfOrigin = @event.Stock.CountryOfOrigin;
            ExpiryDate = @event.Stock.ExpiryDate;
            Quantity = @event.Stock.Quantity;
        }

        private void Handle(StockUpdated @event)
        {
            Id = @event.Stock.Id;
            LicensePlate = @event.Stock.LicensePlate;
            Item = @event.Stock.Item;
            Location = @event.Stock.Location;
            Status = @event.Stock.Status;
            Batch = @event.Stock.Batch;
            CountryOfOrigin = @event.Stock.CountryOfOrigin;
            ExpiryDate = @event.Stock.ExpiryDate;
            Quantity = @event.Stock.Quantity;
        }

        private void Handle(StockDeleted @event)
        {
            Status = "closed";
            Quantity = 0;
            Location = string.Empty;
        }
        public void ReconstituteFromHistory(IEnumerable<IEvent> stockEvents)
        {
            foreach (var stocEvent in stockEvents)
            {
                HandleEvent(stocEvent);
            }
        }
    }
}
