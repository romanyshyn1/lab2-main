using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankExchangeRatesAPI.Models
{
    public class ExchangeRatesItem
    {
        public long Id { get; set; }
        public string? Currency { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
    }
}
