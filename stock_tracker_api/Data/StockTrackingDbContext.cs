
using Microsoft.EntityFrameworkCore;
using StockTrackingApi.Models;

namespace StockTrackingApi.Data 
{
    
    public class StockTrackingDbContext : DbContext {
        private readonly ILogger<StockTrackingDbContext> _logger;

        public StockTrackingDbContext(DbContextOptions<StockTrackingDbContext> options, ILogger<StockTrackingDbContext> logger) 
        : base (options) {
            _logger = logger;
            _logger.LogInformation("StockTrackingDbContext initialized.");            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                _logger.LogWarning("DbContext is not properly configured!");
            }
        }        
        
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Dividend> Dividends { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ProfitLoss> ProfitLosses { get; set; }

    }
}