using AnalisisVentasApi;
using AnalisisVentasApi.Endpoints;
using AnalisisVentasApi.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Registra MongoDB para compartir el mismo cliente durante la ejecución de la API.
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDB:ConnectionString"];

    return new MongoClient(string.IsNullOrWhiteSpace(connectionString)
        ? "mongodb://localhost:27017"
        : connectionString);
});

builder.Services.AddSingleton<VentasRepository>();
builder.Services.AddSingleton<AnalisisVentasService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// La página principal abre el panel; los endpoints siguen devolviendo JSON.
app.MapGet("/", () => Results.Redirect("/index.html"));

// Cada grupo mantiene sus rutas organizadas en un archivo separado.
app.MapVentasEndpoint();
app.MapAnalisisEndpoint();

app.Run();

public partial class Program;
