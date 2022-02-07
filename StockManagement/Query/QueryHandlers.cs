using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{


    public class QueryHandler
    : IQueryHandler<FindStockByIdQuery, FindStockByIdQueryResult>, IQueryHandler<GetAllStockQuery, GetAllStockQueryResult>,
        IQueryHandler<FindSupplierByIdQuery, FindSupplierByIdQueryResult>, IQueryHandler<GetAllSuppliersQuery, GetAllSuppliersQueryResult>
    {
        private readonly StockContext db;

        public QueryHandler(StockContext db)
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
        public FindSupplierByIdQueryResult Handle(FindSupplierByIdQuery query)
        {
            return new FindSupplierByIdQueryResult(db.Suppliers.FirstOrDefault(x => x.Id == query.Id));
        }

        public GetAllSuppliersQueryResult Handle(GetAllSuppliersQuery query)
        {
            return new GetAllSuppliersQueryResult(db.Suppliers.ToArray());

        }

    }


}
