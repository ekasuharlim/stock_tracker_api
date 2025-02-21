namespace StockTrackingApi.Models
{
    public class Dividend
    {
        public int DividendID { get; set; }
        public int StockID { get; set; }
        public DateTime DividendDate { get; set; }
        public decimal DividendPerShare { get; set; }
    }
}