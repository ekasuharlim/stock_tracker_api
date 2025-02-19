
using Microsoft.EntityFrameworkCore;
using StockTrackingApi.Models;

namespace StockTrackingApi.Data 
{
    public class StockTrackingDbContext : DbContext {

        public StockTrackingDbContext(DbContextOptions<StockTrackingDbContext> options) : base (options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }


    }
}