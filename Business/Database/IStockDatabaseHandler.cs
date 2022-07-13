using System.Threading.Tasks;
using LondonStockExchangeApi.Models;

namespace LondonStockExchangeApi.Business.Database
{
    public interface IStockDatabaseHandler
    {
        Task<bool> SaveStockTransactionData(string tickerSymbol, decimal price, decimal numberOfShares, string traderId);
        Task<StockData> GetStockByTickerSymbol(string tickerSymbol);
        Task<TraderData> GetTraderByTraderId(string traderId);
        Task<ApiSecurityData> GetApiSecurity(string clientKey, string clientSecret);
    }
}
