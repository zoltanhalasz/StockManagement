using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{


    public class StockQueryHandler
    : IQueryHandler<FindStockByIdQuery, FindStockByIdQueryResult>, IQueryHandler<GetAllStockQuery, GetAllStockQueryResult>
    {
        private readonly StockContext db;

        public StockQueryHandler(StockContext db)
        {
            this.db = db;
        }

        public FindStockByIdQueryResult Handle(FindStockByIdQuery query)
        {
            return new FindStockByIdQueryResult(db.Stocks.FirstOrDefault(x => x.Id == query.Id));
        }

        public GetAllStockQueryResult Handle(GetAllStockQuery query)
        {
            return new GetAllStockQueryResult(db.Stocks.Where(x=> x.Status!="closed").ToArray());
            
        }

 
    }


}
