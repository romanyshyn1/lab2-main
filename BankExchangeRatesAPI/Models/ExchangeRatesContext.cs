using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BankExchangeRatesAPI.Models
{
    public class ExchangeRatesContext : DbContext
    {
        public ExchangeRatesContext(DbContextOptions<ExchangeRatesContext> options) : base(options)
        {

        }

        public DbSet<ExchangeRatesItem> ExchangeRatesItems { get; set; } = null;
    }
}
