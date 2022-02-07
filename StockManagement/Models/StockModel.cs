

using System;
using System.Collections.Generic;

namespace StockManagement.Models
{

    public class StockModel
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public string Item { get; set; }
        public string Location{ get; set; }
        public string Status { get; set; }
        public string Batch{ get; set; }
        public string CountryOfOrigin{ get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
    }

    public class SupplierModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }

    public class SupplierStock
    {
        public Guid SupplierId { get; set; }
        public Guid StockId { get; set; }
    }

}
