using System;
namespace Fixer.IO.CcyCnvrtr.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(string from, string to, double rate, DateTime date)
        {
            From = from;
            To = to;
            Rate = rate;
            Date = date;
        }

        public string From { get; }

        public string To { get; }

        public double Rate { get; }

        public DateTime Date { get; set; }

        public double Convert(double amount)
        {
            return amount * Rate;
        }
    }
}
