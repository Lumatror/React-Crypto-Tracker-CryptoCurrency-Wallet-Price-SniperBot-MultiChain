using Bitget.Net.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoRate.CryptoExchanges
{
    internal class BitgetExchange : ICryptoConnect
    {
        public event Action<decimal> RateUpdated;
        BitgetSocketClient client;
        public void Connect()
        {
            client = new BitgetSocketClient();
        }

        public async Task GetRate()
        {
            try
            {
                decimal lastPrice;
                Connect();

                var subResult = await client.SpotApi.SubscribeToTickerUpdatesAsync("BTCUSDT", async (update) =>
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
