using AnalisisVentasApi.Services;

namespace AnalisisVentasApi.Endpoints;

public static class AnalisisEndpoint
{
    // Rutas que devuelven los distintos reportes comerciales.
    public static void MapAnalisisEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/analisis").WithTags("Análisis");

        group.MapGet("/resumen", GetResumen);
        group.MapGet("/productos-mas-vendidos", GetProductosMasVendidos);
        group.MapGet("/ingresos-por-categoria", GetIngresosPorCategoria);
        group.MapGet("/ventas-por-ciudad", GetVentasPorCiudad);
        group.MapGet("/vendedores", GetDesempenoVendedores);
        group.MapGet("/reporte", GetReporte);
    }

    private static async Task<IResult> GetResumen(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerResumen(ventas));
    }

    private static async Task<IResult> GetProductosMasVendidos(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerProductosMasVendidos(ventas));
    }

    private static async Task<IResult> GetIngresosPorCategoria(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerIngresosPorCategoria(ventas));
    }

    private static async Task<IResult> GetVentasPorCiudad(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerVentasPorCiudad(ventas));
    }

    private static async Task<IResult> GetDesempenoVendedores(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerDesempenoVendedores(ventas));
    }

    private static async Task<IResult> GetReporte(
        VentasRepository repository,
        AnalisisVentasService analisis)
    {
        var ventas = await repository.ObtenerTodasAsync();
        return TypedResults.Ok(analisis.ObtenerReporte(ventas));
    }
}
