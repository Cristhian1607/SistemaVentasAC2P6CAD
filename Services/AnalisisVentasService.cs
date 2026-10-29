namespace AnalisisVentasApi.Services;

public class AnalisisVentasService
{
    // Calcula indicadores generales usando solo ventas completadas.
    public object ObtenerResumen(List<Venta> ventas)
    {
        var completadas = ventas.Where(v => v.Estado == "Completada").ToList();
        var totales = completadas.Select(v => v.Total).ToList();

        return new
        {
            cantidadVentas = ventas.Count,
            ventasCompletadas = completadas.Count,
            ventasCanceladas = ventas.Count(v => v.Estado == "Cancelada"),
            facturacionTotal = totales.Sum(),
            ticketPromedio = totales.Count > 0 ? totales.Average() : 0,
            ventaMayor = totales.Count > 0 ? totales.Max() : 0,
            ventaMenor = totales.Count > 0 ? totales.Min() : 0,
            ciudadesAtendidas = completadas.Select(v => v.Cliente.Ciudad).Distinct().Count(),
            ventasConPromocion = completadas.Count(v => v.TienePromocion),
            moneda = completadas.Select(v => v.Moneda).Distinct().FirstOrDefault() ?? "CRC"
        };
    }

    public object ObtenerProductosMasVendidos(List<Venta> ventas)
    {
        // SelectMany junta los productos de varias ventas antes de agruparlos.
        return ventas
            .Where(v => v.Estado == "Completada")
            .SelectMany(v => v.Productos)
            .GroupBy(p => new { p.IdProducto, p.Nombre, p.Categoria })
            .Select(grupo => new
            {
                grupo.Key.IdProducto,
                grupo.Key.Nombre,
                grupo.Key.Categoria,
                unidadesVendidas = grupo.Sum(p => p.Cantidad),
                ingresos = grupo.Sum(p => p.Cantidad * p.PrecioUnitario * (1 - p.Descuento / 100))
            })
            .OrderByDescending(p => p.unidadesVendidas)
            .ThenBy(p => p.Nombre)
            .ToList();
    }

    public object ObtenerIngresosPorCategoria(List<Venta> ventas)
    {
        return ventas
            .Where(v => v.Estado == "Completada")
            .SelectMany(v => v.Productos)
            .GroupBy(p => p.Categoria)
            .Select(grupo => new
            {
                categoria = grupo.Key,
                unidadesVendidas = grupo.Sum(p => p.Cantidad),
                ingresos = grupo.Sum(p => p.Cantidad * p.PrecioUnitario * (1 - p.Descuento / 100))
            })
            .OrderByDescending(c => c.ingresos)
            .ToList();
    }

    public object ObtenerVentasPorCiudad(List<Venta> ventas)
    {
        // Esta consulta usa la sintaxis LINQ con from, where, group y select.
        var ventasPorCiudad =
            from venta in ventas
            where venta.Estado == "Completada"
            group venta by venta.Cliente.Ciudad into grupo
            orderby grupo.Sum(venta => venta.Total) descending
            select new
            {
                ciudad = grupo.Key,
                cantidadVentas = grupo.Count(),
                facturacion = grupo.Sum(venta => venta.Total),
                ticketPromedio = grupo.Average(venta => venta.Total)
            };

        return ventasPorCiudad.ToList();
    }

    public object ObtenerDesempenoVendedores(List<Venta> ventas)
    {
        return ventas
            .Where(v => v.Estado == "Completada")
            .GroupBy(v => new { v.Vendedor.Identificacion, v.Vendedor.Nombre, v.Vendedor.Sucursal })
            .Select(grupo => new
            {
                grupo.Key.Identificacion,
                grupo.Key.Nombre,
                grupo.Key.Sucursal,
                ventasCompletadas = grupo.Count(),
                facturacion = grupo.Sum(v => v.Total),
                ticketPromedio = grupo.Average(v => v.Total)
            })
            .OrderByDescending(v => v.facturacion)
            .ToList();
    }

    public object ObtenerReporte(List<Venta> ventas)
    {
        return new
        {
            resumen = ObtenerResumen(ventas),
            productosMasVendidos = ObtenerProductosMasVendidos(ventas),
            ingresosPorCategoria = ObtenerIngresosPorCategoria(ventas),
            ventasPorCiudad = ObtenerVentasPorCiudad(ventas),
            desempenoVendedores = ObtenerDesempenoVendedores(ventas),
            conclusiones = ObtenerConclusiones(ventas)
        };
    }

    private static List<string> ObtenerConclusiones(List<Venta> ventas)
    {
        // Identifica la ciudad y el producto con mejores resultados.
        var completadas = ventas.Where(v => v.Estado == "Completada").ToList();
        var mejorCiudad = completadas
            .GroupBy(v => v.Cliente.Ciudad)
            .Select(g => new { Ciudad = g.Key, Facturacion = g.Sum(v => v.Total) })
            .OrderByDescending(g => g.Facturacion)
            .FirstOrDefault();
        var productoMasVendido = completadas
            .SelectMany(v => v.Productos)
            .GroupBy(p => p.Nombre)
            .Select(g => new { Nombre = g.Key, Unidades = g.Sum(p => p.Cantidad) })
            .OrderByDescending(g => g.Unidades)
            .FirstOrDefault();

        var conclusiones = new List<string>();
        if (mejorCiudad is not null)
        {
            conclusiones.Add($"La ciudad con mayor facturación es {mejorCiudad.Ciudad}.");
        }

        if (productoMasVendido is not null)
        {
            conclusiones.Add($"El producto con más unidades vendidas es {productoMasVendido.Nombre}.");
        }

        conclusiones.Add($"Se analizaron {completadas.Count} ventas completadas.");
        return conclusiones;
    }
}
