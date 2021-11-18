using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fixer.IO.CcyCnvrtr.Models;
using Newtonsoft.Json.Linq;

namespace Fixer.IO.CcyCnvrtr
{
    public interface IFixerManager
    {
        double Convert(string from, string to, double amount, DateTime? date = null);
        Task<double> ConvertAsync(string from, string to, double amount, DateTime? date = null);
        ExchangeRate Rate(string from, string to, DateTime? date = null);
        Task<ExchangeRate> RateAsync(string from, string to, DateTime? date = null);
        Task<string> GetDailyRateAsync(string BaseCurrency = "EUR");
    }
}
