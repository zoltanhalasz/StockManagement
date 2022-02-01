using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{
    public class FindStockByIdQuery : IQuery<StockModel>
    {
        public Guid Id { get; set; }
        
    }

    public class GetAllStockQuery : IQuery<StockModel[]>
    {
        
    }
}
