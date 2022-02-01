using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Command

{
    public class UpdateStockCommandHandler
        : ICommandHandler<UpdateStockCommand>
    {
        private readonly StockContext db;

        public UpdateStockCommandHandler(StockContext db)
        {
            this.db = db;
        }

        public void Handle(UpdateStockCommand command)
        {
            db.Stocks.Update(command.UpdateStock);
            db.SaveChanges();
        }
    }

    public class CreateStockCommandHandler: ICommandHandler<CreateStockCommand>
    {
        private readonly StockContext db;

        public CreateStockCommandHandler(StockContext db)
        {
            this.db = db;
        }

        public void Handle(CreateStockCommand command)
        {
            db.Stocks.Add(command.CreateStock);
            db.SaveChanges();
        }
    }
}
