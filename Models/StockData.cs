namespace LondonStockExchangeApi.Models
{
    public class StockData
    {
        public string StockName { get; set; }
        public string TickerSymbols { get; set; }
        public string IndustryType { get; set; }
        public string SPRating { get; set; }

        public string CurrentPrice { get; set; }
    }
}
