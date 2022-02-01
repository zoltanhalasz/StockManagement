using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{
    public class FindStockByIdQueryHandler
        : IQueryHandler<FindStockByIdQuery, StockModel>
    {
        private readonly StockContext db;

        public FindStockByIdQueryHandler(StockContext db)
        {
            this.db = db;
        }

        public StockModel Handle(FindStockByIdQuery query)
        {
            return db.Stocks.FirstOrDefault(x=> x.Id == query.Id);
        }
    }

    public class GetAllStockQueryQueryHandler
       : IQueryHandler<GetAllStockQuery, StockModel[]>
    {
        private readonly StockContext db;

        public GetAllStockQueryQueryHandler(StockContext db)
        {
            this.db = db;
        }

        public StockModel[] Handle(GetAllStockQuery query)
        {
            return db.Stocks.ToArray();
        }
    }
}
