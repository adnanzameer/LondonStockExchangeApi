using System;
using System.Threading.Tasks;
using LondonStockExchangeApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LondonStockExchangeApi.Business.Database
{
    public class StockDatabaseHandler : IStockDatabaseHandler
    {
        private readonly IMemoryCache _cache;

        public StockDatabaseHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> SaveStockTransactionData(string tickerSymbol, decimal price, decimal numberOfShares, string traderId)
        {
            // Save data to database
            return true;
        }

        public async Task<StockData> GetStockByTickerSymbol(string tickerSymbol)
        {
            // Get Object From Cache 
            // If not not available, get it from database
            // Add it to Cache 

            _cache.TryGetValue(tickerSymbol, out StockData stockData);

            if (stockData != null)
            {
                return stockData;
            }

            // var stockData = get stock from database using tickerSymbol;
            // if (stockData != null)

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(3));

            _cache.Set(tickerSymbol, stockData, cacheEntryOptions);

             return stockData;
        }

        public async Task<TraderData> GetTraderByTraderId(string traderId)
        {
            // Get Object From Cache 
            // If not not available, get it from database
            // Add it to Cache 
            _cache.TryGetValue(traderId, out TraderData trader);

            if (trader != null)
            {
                return trader;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(3));

            _cache.Set(traderId, trader, cacheEntryOptions);

            return trader;
        }

        public async Task<ApiSecurityData> GetApiSecurity(string clientKey, string clientSecret)
        {
            // var securityData = get security data from database using clientKey & clientSecret;
            // if (securityData != null && securityData.Active)
            // return traderId

            return null;
        }
    }
}
