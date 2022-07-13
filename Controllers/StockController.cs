using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LondonStockExchangeApi.Business.Database;
using LondonStockExchangeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LondonStockExchangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockDatabaseHandler _stockDatabaseHandler;
        public StockController(IStockDatabaseHandler stockDatabaseHandler)
        {
            _stockDatabaseHandler = stockDatabaseHandler;
        }

        #region APIs

        [HttpGet("Test")]
        public string Test()
        {
            return "test";
        }

        [HttpPost("Transaction")]
        public async Task<IActionResult> RecordStockTransaction(string tickerSymbol, decimal price, decimal numberOfShares, string traderId)
        {
            // Check if the Security of API call

            var securityData = await CheckApiSecurity();
            if (securityData != null)
            {
                // Check if the trader Id is valid & matches with api security credentials
                if (!string.IsNullOrEmpty(traderId) && securityData.TraderId.Equals(traderId, StringComparison.InvariantCultureIgnoreCase))
                {
                    var trader = await _stockDatabaseHandler.GetTraderByTraderId(traderId);

                    if (trader == null)
                        return NotFound();
                }

                // Check if ticker symbol is valid
                if (!string.IsNullOrEmpty(tickerSymbol))
                {
                    var stock = await _stockDatabaseHandler.GetTraderByTraderId(tickerSymbol);

                    if (stock == null)
                        return NotFound();
                }

                if (price <= 0.0m)
                {
                    return NotFound();
                }

                if (numberOfShares < 0.0m)
                {
                    return NotFound();
                }

                // Check through SDK if price and number of shares ratio translation is allowed against the stock in the system 
                if (true)
                {
                    var status = await _stockDatabaseHandler.SaveStockTransactionData(tickerSymbol, price, numberOfShares, traderId);

                    if (status)
                        return Ok("Success");
                }
            }
            return NoContent();
        }



        [HttpGet("Information")]
        public async Task<IActionResult> StocksInformation([FromBody] List<string> tickerSymbols)
        {
            if (tickerSymbols == null || !tickerSymbols.Any())
                return NotFound();

            var securityData = await CheckApiSecurity();

            if (securityData != null)
            {
                var stockList = await GetStocks(tickerSymbols);

                return Ok(stockList);
            }

            return NoContent();
        }

        #endregion

        #region Helpers

        private string GetHeaderData(string headerKey)
        {
            Request.Headers.TryGetValue(headerKey, out var headerValue);
            return headerValue;
        }

        private async Task<ApiSecurityData> CheckApiSecurity()
        {
            var clientKey = GetHeaderData("Client-Id");
            var clientSecret = GetHeaderData("Client-Secret");
            if (!string.IsNullOrEmpty(clientKey) && !string.IsNullOrEmpty(clientSecret))
            {
                return await _stockDatabaseHandler.GetApiSecurity(clientKey, clientSecret);
            }

            return null;
        }

        private async Task<IEnumerable<StockData>> GetStocks(IEnumerable<string> tickerSymbols)
        {
            if (tickerSymbols != null)
            {
                var list = new List<StockData>();
                foreach (var stockSymbol in tickerSymbols)
                {
                    var stock = await _stockDatabaseHandler.GetStockByTickerSymbol(stockSymbol);

                    if (stock != null) list.Add(stock);
                }

                return list;
            }

            return null;
        }
        #endregion
    }
}
