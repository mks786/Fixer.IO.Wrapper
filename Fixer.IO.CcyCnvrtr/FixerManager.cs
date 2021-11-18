using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fixer.IO.CcyCnvrtr.Config;
using Fixer.IO.CcyCnvrtr.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Fixer.IO.CcyCnvrtr
{
    internal class FixerManager : IFixerManager
    {
        private readonly FixerConfig _config;
        private readonly ILogger<FixerManager> _logger;
        private readonly bool _isLoggingEnabled = false;

        public FixerManager(FixerConfig config, ILogger<FixerManager> logger = null)
        {
            _config = config ?? throw new ArgumentNullException();
            _logger = logger;
            _isLoggingEnabled = _logger != null;
        }

        public double Convert(string from, string to, double amount, DateTime? date = null)
        {
            return GetRate(from, to, date).Convert(amount);
        }

        public async Task<double> ConvertAsync(string from, string to, double amount, DateTime? date = null)
        {
            return (await GetRateAsync(from, to, date)).Convert(amount);
        }

        public ExchangeRate Rate(string from, string to, DateTime? date = null)
        {
            return GetRate(from, to, date);
        }

        public async Task<ExchangeRate> RateAsync(string from, string to, DateTime? date = null)
        {
            return await GetRateAsync(from, to, date);
        }

        private ExchangeRate GetRate(string from, string to, DateTime? date = null)
        {
            from = from.ToUpper();
            to = to.ToUpper();

            if (!Symbols.IsValid(from))
                throw new ArgumentException("Symbol not found for provided currency", "from");

            if (!Symbols.IsValid(to))
                throw new ArgumentException("Symbol not found for provided currency", "to");

            var url = GetFixerUrl(date);

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                return ParseData(response.Content.ReadAsStringAsync().Result, from, to);
            }
        }

        private async Task<ExchangeRate> GetRateAsync(string from, string to, DateTime? date = null)
        {
            from = from.ToUpper();
            to = to.ToUpper();

            if (!Symbols.IsValid(from))
                throw new ArgumentException("Symbol not found for provided currency", "from");

            if (!Symbols.IsValid(to))
                throw new ArgumentException("Symbol not found for provided currency", "to");

            var url = GetFixerUrl(date);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return ParseData(await response.Content.ReadAsStringAsync(), from, to);
            }
        }

        private ExchangeRate ParseData(string data, string from, string to)
        {
            // Parse JSON
            var root = JObject.Parse(data);

            var rates = root.Value<JObject>("rates");
            var fromRate = rates.Value<double>(from);
            var toRate = rates.Value<double>(to);

            var rate = toRate / fromRate;

            // Parse returned date
            // Note: This may be different to the requested date as Fixer will return the closest available
            var returnedDate = DateTime.ParseExact(root.Value<string>("date"), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);

            return new ExchangeRate(from, to, rate, returnedDate);
        }

        private string GetFixerUrl(DateTime? date = null, bool GetDailyRate = false)
        {
            var dateString = date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "latest";
            if(!GetDailyRate)
            return $"{_config.BaseUri}{dateString}?access_key={_config.APIKey}";
            else
                return $"{_config.BaseUri}latest?access_key={_config.APIKey}&callback=MY_FUNCTION";
        }

        public async Task<string> GetDailyRateAsync(string BaseCurrency="EUR")
        {
            var url = GetFixerUrl(GetDailyRate:true);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
        }

    }
}
