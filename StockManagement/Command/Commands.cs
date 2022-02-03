using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Command
{
    public class UpdateStockCommand : ICommand
    {
        public StockModel UpdateStock { get; set; }
        
    }

    public class CreateStockCommand : ICommand
    {
        public StockModel CreateStock { get; set; }
    }

    public class DeleteStockCommand : ICommand
    {
        public Guid Id { get; set; }
    }

}
