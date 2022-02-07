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

    public class UpdateSupplierCommand : ICommand
    {
        public SupplierModel UpdateSupplier { get; set; }

    }

    public class CreateSupplierCommand : ICommand
    {
        public SupplierModel CreateSupplier { get; set; }
    }

    public class DeleteSupplierCommand : ICommand
    {
        public Guid SupplierId { get; set; }
    }

    public class AddStockToSupplierCommand : ICommand
    {
        public Guid SupplierId { get; set; }
        public Guid StockId { get; set; }

    }

    public class RemoveStockFromSupplierCommand : ICommand
    {
        public Guid SupplierId { get; set; }
        public Guid StockId { get; set; }

    }

}
