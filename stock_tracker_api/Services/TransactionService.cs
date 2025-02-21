
using Microsoft.EntityFrameworkCore;
using StockTrackingApi.Data;
using StockTrackingApi.Models;

namespace StockTrackingApi.Services
{
    public class TransactionService
    {
        private readonly StockTrackingDbContext _context;
        public TransactionService(StockTrackingDbContext context)
        {
            _context = context;
        }

        public async Task ProcessSaleAsync(Transaction saleTransaction)
        {
            using var transaction =  await _context.Database.BeginTransactionAsync();

            var stockHoldings = await _context.Transactions
                .Where(t => t.StockID == saleTransaction.StockID 
                            && t.UserID == saleTransaction.UserID 
                            && t.TransactionType == "Buy")
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();

            int remainingToSell = saleTransaction.Quantity;
            foreach (var purchase in stockHoldings)
            {
                if (remainingToSell <= 0) break;

                int sellQuantity = remainingToSell > purchase.Quantity ? purchase.Quantity : remainingToSell;
                decimal profitOrLoss = (saleTransaction.PricePerShare - purchase.PricePerShare) * sellQuantity;

                _context.ProfitLosses.Add(new ProfitLoss
                {
                    UserID = saleTransaction.UserID,
                    StockID = saleTransaction.StockID,
                    SaleTransactionID = saleTransaction.TransactionID,
                    PurchaseTransactionID = purchase.TransactionID,
                    SoldQuantity = sellQuantity,
                    PurchasePrice = purchase.PricePerShare,
                    SalePrice = saleTransaction.PricePerShare,
                    Profit = profitOrLoss > 0 ? profitOrLoss : null,
                    Loss = profitOrLoss < 0 ? -profitOrLoss : null
                });

                purchase.Quantity -= sellQuantity;

                _context.Transactions.Update(purchase);

                remainingToSell -= sellQuantity;
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }   
}