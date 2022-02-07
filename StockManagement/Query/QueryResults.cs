using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{
    public interface IQueryResult
    {
     
    }
    public class FindStockByIdQueryResult : IQueryResult
    {
        public StockModel Stock { get; set; }

        public FindStockByIdQueryResult(StockModel stock    )
        {
            Stock = stock;
        }
    }

    public class GetAllStockQueryResult: IQueryResult
    {
        public StockModel[] Stocks { get; set; }

        public GetAllStockQueryResult(StockModel[] stocks)
        {
            Stocks = stocks;
        }
 
    }

    public class FindSupplierByIdQueryResult : IQueryResult
    {
        public SupplierModel Supplier{ get; set; }

        public FindSupplierByIdQueryResult(SupplierModel Supplier)
        {
            this.Supplier = Supplier;
        }
    }

    public class GetAllSuppliersQueryResult : IQueryResult
    {
        public SupplierModel[] Suppliers { get; set; }

        public GetAllSuppliersQueryResult(SupplierModel[] Suppliers)
        {
            this.Suppliers = Suppliers;
        }

    }

}
