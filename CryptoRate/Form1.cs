using CryptoRate.CryptoExchanges;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoRate
{
    public partial class Form1 : Form
    {
        BybitExchange bybitExchange;
        BinanceExchange binanceExchange;
        BitgetExchange bitgetExchange;
        KucoinExchange kucoinExchange;
        bool isFormClosing = false;
        public Form1()
        {
            InitializeComponent();

            bybitExchange = new BybitExchange();
            binanceExchange = new BinanceExchange();
            bitgetExchange = new BitgetExchange();
            kucoinExchange = new KucoinExchange();
            bybitExchange.RateUpdated += OnBybitRateUpdated;
            binanceExchange.RateUpdated += OnBinanceRateUpdated;
            bitgetExchange.RateUpdated += OnBitgetRateUpdated;
            kucoinExchange.RateUpdated += OnKucoinRateUpdated;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await GetRates();
        }

        private async Task GetRates()
        {
            if (!isFormClosing)
            {
                await kucoinExchange.GetRate();
                await bitgetExchange.GetRate();
                await binanceExchange.GetRate();
                await bybitExchange.GetRate();
            }
        }

        private void OnBybitRateUpdated(decimal rate)
        {
            if (!isFormClosing)
                BeginInvoke(new Action(() => bybitData.Text = rate.ToString()));
        }

        private void OnBinanceRateUpdated(decimal rate)
        {
            if (!isFormClosing)
                BeginInvoke(new Action(() => binanceData.Text = rate.ToString()));
        }

        private void OnBitgetRateUpdated(decimal rate)
        {
            if (!isFormClosing)
                BeginInvoke(new Action(() => bitgetData.Text = rate.ToString()));
        }

        private void OnKucoinRateUpdated(decimal rate)
        {
            if (!isFormClosing)
                BeginInvoke(new Action(() => kucoinData.Text = rate.ToString()));
        }
        private async Task UnsubscribeRates()
        {
            await kucoinExchange.UnsubscribeGetRate();
            await binanceExchange.UnsubscribeGetRate();
            await bitgetExchange.UnsubscribeGetRate();
            await bybitExchange.UnsubscribeGetRate();
        }
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            await UnsubscribeRates();
            isFormClosing = true;          
        }
    }
}
