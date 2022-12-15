using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ExchangeRatesDb>(opt => opt.UseInMemoryDatabase("ExchangeRatesList"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/exchangeRatesitems", async (ExchangeRatesDb db) =>
    await db.ExchangeRates.ToListAsync());

app.MapGet("/exchangeRatesitems/complete", async (ExchangeRatesDb db) =>
    await db.ExchangeRates.Where(t => t.InStock).ToListAsync());

app.MapGet("/exchangeRatesitems/{id}", async (int id, ExchangeRatesDb db) =>
    await db.ExchangeRates.FindAsync(id)
        is ExchangeRates exchangeRates
            ? Results.Ok(exchangeRates)
            : Results.NotFound());

app.MapPost("/exchangeRatesitems", async (ExchangeRates exchangeRates, ExchangeRatesDb db) =>
{
    db.ExchangeRates.Add(exchangeRates);
    await db.SaveChangesAsync();

    return Results.Created($"/exchangeRatesitems/{exchangeRates.Id}", exchangeRates);
});

app.MapPut("/exchangeRatesitems/{id}", async (int id, ExchangeRates inputExchangeRates, ExchangeRatesDb db) =>
{
    var exchangeRates = await db.ExchangeRates.FindAsync(id);

    if (exchangeRates is null) return Results.NotFound();

    exchangeRates.Currency = inputExchangeRates.Currency;
    exchangeRates.Buy = inputExchangeRates.Buy;
    exchangeRates.Sell = inputExchangeRates.Sell;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/exchangeRatesitems/{id}", async (int id, ExchangeRatesDb db) =>
{
    if (await db.ExchangeRates.FindAsync(id) is ExchangeRates exchangeRates)
    {
        db.ExchangeRates.Remove(exchangeRates);
        await db.SaveChangesAsync();
        return Results.Ok(exchangeRates);
    }

    return Results.NotFound();
});

app.Run();

class ExchangeRates
{
    public long Id { get; set; }
    public string? Currency { get; set; }
    public decimal Buy { get; set; }
    public decimal Sell { get; set; }
    public bool InStock { get; set; }
}

class ExchangeRatesDb : DbContext
{
    public ExchangeRatesDb(DbContextOptions<ExchangeRatesDb> options)
        : base(options) { }

    public DbSet<ExchangeRates> ExchangeRates => Set<ExchangeRates>();
}