using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;
using System.Threading.Tasks;

namespace WebJobStocks.Models
{
    public interface IStockEventHandler
    {
        Task Handle(IEvent @event);
    }

    public class StockEventHandler: IStockEventHandler
    {        

        public async Task Handle(IEvent @event)
        {           
            
            Console.WriteLine(@event);
        }
    }
}
