using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{
    public class FindStockByIdQuery : IQuery<FindStockByIdQueryResult>
    {
        public Guid Id { get; set; }
        
    }

    public class GetAllStockQuery: IQuery<GetAllStockQueryResult>
    {
        
    }

    public class FindSupplierByIdQuery : IQuery<FindSupplierByIdQueryResult>
    {
        public Guid Id { get; set; }

    }

    public class GetAllSuppliersQuery : IQuery<GetAllSuppliersQueryResult>
    {

    }
}
