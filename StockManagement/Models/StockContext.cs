using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockManagement.Models
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> dbcontextoption)
            : base(dbcontextoption)
        { }

        public DbSet<StockModel> Stocks { get; set; }

        public DbSet<SupplierModel> Suppliers { get; set; }
        public DbSet<SupplierStock> SupplierStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierStock>()
                .HasKey(nameof(SupplierStock.SupplierId), nameof(SupplierStock.StockId));
        }
    }
}
