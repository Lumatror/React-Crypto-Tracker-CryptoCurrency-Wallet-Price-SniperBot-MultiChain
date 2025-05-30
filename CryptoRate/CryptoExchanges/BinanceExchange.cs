using Binance.Net.Clients;
using CryptoExchange.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoRate.CryptoExchanges
{
    internal class BinanceExchange : ICryptoConnect
    {
        public event Action<decimal> RateUpdated;
        private BinanceSocketClient client;
        public void Connect()
        {
            client = new BinanceSocketClient();
        }

        public async Task GetRate()
        {
            try
            {
                decimal lastPrice;
                Connect();

                var subResult = await client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTCUSDT", async (update) =>
                {
                    lastPrice = Convert.ToDecimal(update.Data.LastPrice);
                    RateUpdated?.Invoke(lastPrice);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data.ToString());
            }
        }

        public async Task UnsubscribeGetRate()
        {
            await client.SpotApi.UnsubscribeAllAsync();
        }
    }
}
