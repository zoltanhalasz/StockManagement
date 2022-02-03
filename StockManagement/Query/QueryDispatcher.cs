using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Query
{
    public interface IQueryDispatcher
    {
        IQueryResult Dispatch<TQuery, IQueryResult>(TQuery query) where TQuery : IQuery<IQueryResult>;
    }
    public class QueryDispatcher : IQueryDispatcher
    {
        IServiceProvider service;
        public QueryDispatcher(IServiceProvider service)
        {
            this.service = service;
        }

        public IQueryResult Dispatch<TQuery, IQueryResult>(TQuery query) where TQuery : IQuery<IQueryResult> 
        {
            var myType = typeof(IQueryHandler<TQuery, IQueryResult>);
            var myService = service.GetService(myType);
            return (myService as IQueryHandler<TQuery, IQueryResult>).Handle(query);
        }

    }


}
