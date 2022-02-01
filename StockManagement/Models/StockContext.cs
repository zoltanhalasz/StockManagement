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

    }
}
