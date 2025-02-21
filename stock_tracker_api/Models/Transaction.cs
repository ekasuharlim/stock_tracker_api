namespace StockTrackingApi.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int StockID { get; set; }
        public DateTime TransactionDate { get; set; }
        public required string TransactionType { get; set; } // Buy/Sell
        public int Quantity { get; set; }
        public decimal PricePerShare { get; set; }
        public decimal Amount => Quantity * PricePerShare;
    }

}