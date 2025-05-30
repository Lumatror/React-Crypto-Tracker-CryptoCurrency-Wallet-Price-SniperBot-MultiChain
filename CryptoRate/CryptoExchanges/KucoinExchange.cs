using CryptoExchange.Net.Interfaces;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects.Models.Spot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoRate.CryptoExchanges
{
    internal class KucoinExchange : ICryptoConnect
    {
        public event Action<decimal> RateUpdated;
        KucoinSocketClient client;
        public void Connect()
        {
            client = new KucoinSocketClient();
        }
 
        public async Task GetRate()
        {
            try
            {
                decimal lastPrice;
                Connect();

                var subResult = await client.SpotApi.SubscribeToTickerUpdatesAsync("BTC-USDT", async (update) =>
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
