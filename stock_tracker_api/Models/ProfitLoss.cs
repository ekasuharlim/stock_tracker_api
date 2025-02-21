namespace StockTrackingApi.Models
{
    public class ProfitLoss
    {
        public int ProfitLossID { get; set; }
        public int UserID { get; set; }
        public int StockID { get; set; }
        public int SaleTransactionID { get; set; }
        public int PurchaseTransactionID { get; set; }
        public int SoldQuantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? Profit { get; set; }
        public decimal? Loss { get; set; }
    }
}
