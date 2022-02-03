using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Events
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
}
