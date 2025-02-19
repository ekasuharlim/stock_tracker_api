namespace StockTrackingApi.Models
{
    public class Stock
    {
        public int StockID { get; set; }
        public required string StockCode { get; set; }
        public required string StockName { get; set; }
    }
}