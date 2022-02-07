
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebJobStocks.Models
{
    public class IEvent
    {
    }
    public class StockCreated: IEvent
    {
        public StockModel Stock { get; set; }
    }
    public class StockUpdated : IEvent
    {
        public StockModel Stock { get; set; }
    }

    public class StockDeleted : IEvent
    {
        public Guid Id { get; set; }
    }

    public class SupplierCreated : IEvent
    {
        public SupplierModel Supplier { get; set; }
    }
    public class SupplierUpdated : IEvent
    {
        public SupplierModel Supplier { get; set; }
    }

    public class SupplierDeleted : IEvent
    {
        public Guid SupplierId { get; set; }
    }

    public class StockAddedToSupplier : IEvent
    {
        public Guid SupplierId { get; set; }

        public Guid StockId { get; set; }
    }

    public class StockRemovedFromSupplier : IEvent
    {
        public Guid SupplierId { get; set; }

        public Guid StockId { get; set; }
    }
    public class PublishModel
    {
        public string Payload { get; set; }
        public string EventType { get; set; }
    }

}
