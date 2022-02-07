using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;
using System.Threading.Tasks;
using System.Linq;

namespace WebJobStocks.Models
{
    public interface IEventHandler
    {
        Task Handle(IEvent @event);
    }

    public class EventHandler: IEventHandler
    {

        private readonly StockContext db;
        public EventHandler(StockContext db)
        {
            this.db = db;
        }

        public async Task Handle(IEvent eventDeserialized)
        {
            if (eventDeserialized is StockCreated)
            {
                var addStock = (eventDeserialized as StockCreated).Stock;
                db.Stocks.Add(addStock);
                db.SaveChanges();
            }

            if (eventDeserialized is StockUpdated)
            {
                var dbEntity = await db.Stocks.FirstOrDefaultAsync(x => x.Id == (eventDeserialized as StockUpdated).Stock.Id);
                dbEntity.Item = (eventDeserialized as StockUpdated).Stock.Item;
                dbEntity.Batch= (eventDeserialized as StockUpdated).Stock.Batch;
                dbEntity.Location = (eventDeserialized as StockUpdated).Stock.Location;
                dbEntity.LicensePlate= (eventDeserialized as StockUpdated).Stock.LicensePlate;
                dbEntity.Quantity = (eventDeserialized as StockUpdated).Stock.Quantity;
                dbEntity.Status = (eventDeserialized as StockUpdated).Stock.Status;
                dbEntity.CountryOfOrigin = (eventDeserialized as StockUpdated).Stock.CountryOfOrigin;

                db.Stocks.Append(dbEntity);
                db.SaveChanges();
            }

            if (eventDeserialized is StockDeleted)
            {
                var dbEntity = await db.Stocks.FirstOrDefaultAsync(x => x.Id == (eventDeserialized as StockDeleted).Id);
                if (dbEntity != null)
                {
                    dbEntity.Quantity = 0;
                    dbEntity.Status = "closed";
                    dbEntity.Location = string.Empty;
                    db.Stocks.Update(dbEntity);
                    db.SaveChanges();
                }

            }

            if (eventDeserialized is SupplierCreated)
            {
                var addSupplier = (eventDeserialized as SupplierCreated).Supplier;
                db.Suppliers.Add(addSupplier);
                db.SaveChanges();
            }

            if (eventDeserialized is SupplierUpdated)
            {
                db.Suppliers.Update((eventDeserialized as SupplierUpdated).Supplier);
                db.SaveChanges();
            }

            if (eventDeserialized is SupplierDeleted)
            {
                var dbEntity = await db.Suppliers.FirstOrDefaultAsync(x => x.Id == (eventDeserialized as SupplierDeleted).SupplierId);
                if (dbEntity != null)
                {
                    db.Suppliers.Remove(dbEntity);
                    db.SaveChanges();
                }
                var dbSupplierStockEntities = await db.SupplierStocks.Where(x => x.SupplierId == (eventDeserialized as SupplierDeleted).SupplierId).ToListAsync();
                if (dbSupplierStockEntities != null)
                {
                    db.SupplierStocks.RemoveRange(dbSupplierStockEntities);
                    db.SaveChanges();
                }
            }

            if (eventDeserialized is StockAddedToSupplier)
            {
                var dbEntity = await db.SupplierStocks.FirstOrDefaultAsync(x => x.SupplierId == (eventDeserialized as StockAddedToSupplier).SupplierId && x.StockId == (eventDeserialized as StockAddedToSupplier).StockId);
                if (dbEntity == null)
                {
                    db.SupplierStocks.Add(new SupplierStock() { SupplierId = (eventDeserialized as StockAddedToSupplier).SupplierId, StockId = (eventDeserialized as StockAddedToSupplier).StockId });
                    db.SaveChanges();
                }

            }

            if (eventDeserialized is StockRemovedFromSupplier)
            {
                var dbEntity = await db.SupplierStocks.FirstOrDefaultAsync(x => x.SupplierId == (eventDeserialized as StockRemovedFromSupplier).SupplierId && x.StockId == (eventDeserialized as StockRemovedFromSupplier).StockId);
                if (dbEntity != null)
                {
                    db.SupplierStocks.Remove(dbEntity);
                    db.SaveChanges();
                }

            }
        }
    }
}
