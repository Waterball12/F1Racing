using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddHttpClient<F1StandingsService>(options =>
{
    options.DefaultRequestHeaders.Add("x-api-key", builder.Configuration.GetValue<string>("ApiKey"));
});

var app = builder.Build();
app.UseCors("AllowAll");

app.MapGet("/api/f1/standings/{season}", async (int season) => Results.Ok(F1StandingsServiceV2.GetStandings()));

app.Run();

class F1StandingsService
{
    private readonly HttpClient _httpClient;
    public F1StandingsService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<object> GetStandings(int season)
    {
        var url = $"https://pitwall.redbullracing.com/api/standings/drivers/{season}";
        var response = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<object>(response);
    }
}


public class DriverStanding
{
    public int Position { get; set; }
    public string Driver { get; set; }
    public string Nationality { get; set; }
    public string Car { get; set; }
    public int Points { get; set; }
}

public static class F1StandingsServiceV2
{
    private static readonly List<DriverStanding> standings = new List<DriverStanding>
    {
        new DriverStanding { Position = 1, Driver = "Max Verstappen", Nationality = "NED", Car = "Red Bull Racing Honda RBPT", Points = 437 },
        new DriverStanding { Position = 2, Driver = "Lando Norris", Nationality = "GBR", Car = "McLaren Mercedes", Points = 374 },
        new DriverStanding { Position = 3, Driver = "Charles Leclerc", Nationality = "MON", Car = "Ferrari", Points = 356 },
        new DriverStanding { Position = 4, Driver = "Oscar Piastri", Nationality = "AUS", Car = "McLaren Mercedes", Points = 292 },
        new DriverStanding { Position = 5, Driver = "Carlos Sainz", Nationality = "ESP", Car = "Ferrari", Points = 290 },
        new DriverStanding { Position = 6, Driver = "George Russell", Nationality = "GBR", Car = "Mercedes", Points = 245 },
        new DriverStanding { Position = 7, Driver = "Lewis Hamilton", Nationality = "GBR", Car = "Mercedes", Points = 223 },
        new DriverStanding { Position = 8, Driver = "Sergio Perez", Nationality = "MEX", Car = "Red Bull Racing Honda RBPT", Points = 152 },
        new DriverStanding { Position = 9, Driver = "Fernando Alonso", Nationality = "ESP", Car = "Aston Martin Aramco Mercedes", Points = 70 },
        new DriverStanding { Position = 10, Driver = "Pierre Gasly", Nationality = "FRA", Car = "Alpine Renault", Points = 42 }
    };

    public static List<DriverStanding> GetStandings()
    {
        return standings;
    }
}
