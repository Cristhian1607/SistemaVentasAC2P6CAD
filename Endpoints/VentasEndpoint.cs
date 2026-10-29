using AnalisisVentasApi.Services;

namespace AnalisisVentasApi.Endpoints;

public static class VentasEndpoint
{
    // Rutas para consultar, registrar y cargar ventas de ejemplo.
    public static void MapVentasEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ventas").WithTags("Ventas");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", Create);
        group.MapPost("/cargar-ejemplo", LoadExamples);
    }

    private static async Task<IResult> GetAll(VentasRepository repository)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(ventas);
    }

    private static async Task<IResult> GetById(string id, VentasRepository repository)
    {
        var venta = await repository.ObtenerPorIdAsync(id);

        if (venta is null)
        {
            return TypedResults.NotFound(new { mensaje = "No se encontró la venta." });
        }

        return TypedResults.Ok(venta);
    }

    private static async Task<IResult> Create(Venta nuevaVenta, VentasRepository repository)
    {
        // Se revisan los datos antes de guardar la venta.
        var error = ValidarVenta(nuevaVenta);
        if (error is not null)
        {
            return TypedResults.BadRequest(new { mensaje = error });
        }

        await repository.GuardarAsync(nuevaVenta);
        return TypedResults.Created($"/api/ventas/{nuevaVenta.Id}", nuevaVenta);
    }

    private static async Task<IResult> LoadExamples(VentasRepository repository)
    {
        // Evita insertar las ventas de ejemplo más de una vez.
        var cargadas = await repository.CargarEjemplosSiEstaVacioAsync();

        if (!cargadas)
        {
            return TypedResults.Conflict(new
            {
                mensaje = "La colección ya tiene ventas; no se agregaron ejemplos duplicados."
            });
        }

        return TypedResults.Ok(new { mensaje = "Se cargaron 10 ventas de ejemplo." });
    }

    private static string? ValidarVenta(Venta venta)
    {
        if (venta.Cliente is null || venta.Vendedor is null ||
            venta.Productos is null || venta.Productos.Count == 0)
        {
            return "La venta debe incluir cliente, vendedor y al menos un producto.";
        }

        if (venta.Productos.Any(p =>
                p.Cantidad <= 0 || p.PrecioUnitario < 0 || p.Descuento is < 0 or > 100))
        {
            return "Cada producto debe tener cantidad positiva, precio no negativo y descuento entre 0 y 100.";
        }

        return null;
    }
}
