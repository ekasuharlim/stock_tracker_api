using StockTrackingApi.Models;

namespace StockTrackingApi.Services.Interfaces
{
    public interface ITransactionService
    {
        Task ProcessSaleAsync(Transaction saleTransaction);
    }
}
