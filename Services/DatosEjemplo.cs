namespace AnalisisVentasApi.Services;

public static class DatosEjemplo
{
    // Crea diez ventas con diferentes clientes, productos y estados.
    public static List<Venta> CrearVentas()
    {
        return
        [
            CrearVenta(1, new DateTime(2026, 9, 15, 10, 30, 0), "Completada", "C1001", "Ana Rodríguez", "San José", "Premium", "V015", "Carlos Méndez", "Escazú",
                [Producto("P001", "Laptop", "Tecnología", 2, 450000, 5), Producto("P002", "Mouse", "Tecnología", 3, 12000, 0)],
                936000, 45000, 115830, 1006830, "Sitio web", true),
            CrearVenta(2, new DateTime(2026, 9, 16, 11, 0, 0), "Completada", "C1002", "Luis Vargas", "Heredia", "Regular", "V016", "María Solís", "Heredia",
                [Producto("P001", "Laptop", "Tecnología", 1, 450000, 5), Producto("P002", "Mouse", "Tecnología", 1, 12000, 0)],
                462000, 22500, 57135, 496635, "Tienda física", false),
            CrearVenta(3, new DateTime(2026, 9, 18, 14, 15, 0), "Completada", "C1003", "Sofía Jiménez", "Cartago", "Premium", "V015", "Carlos Méndez", "Escazú",
                [Producto("P003", "Cafetera", "Hogar", 2, 85000, 0), Producto("P004", "Café molido", "Alimentos", 4, 8500, 0)],
                204000, 0, 26520, 230520, "Sitio web", false),
            CrearVenta(4, new DateTime(2026, 9, 20, 9, 45, 0), "Completada", "C1004", "Diego Mora", "Alajuela", "Regular", "V017", "Jorge León", "Alajuela",
                [Producto("P005", "Teléfono", "Tecnología", 1, 320000, 10)],
                320000, 32000, 37440, 325440, "Tienda física", true),
            CrearVenta(5, new DateTime(2026, 9, 22, 16, 20, 0), "Completada", "C1005", "Valeria Castro", "San José", "Premium", "V016", "María Solís", "Heredia",
                [Producto("P006", "Silla de oficina", "Hogar", 3, 78000, 0)],
                234000, 0, 30420, 264420, "Sitio web", false),
            CrearVenta(6, new DateTime(2026, 9, 25, 12, 5, 0), "Completada", "C1006", "Pablo Rojas", "Heredia", "Regular", "V017", "Jorge León", "Alajuela",
                [Producto("P002", "Mouse", "Tecnología", 5, 12000, 5)],
                60000, 3000, 7410, 64410, "Redes sociales", true),
            CrearVenta(7, new DateTime(2026, 9, 28, 13, 30, 0), "Completada", "C1007", "Camila Soto", "Cartago", "Premium", "V015", "Carlos Méndez", "Escazú",
                [Producto("P001", "Laptop", "Tecnología", 1, 450000, 5), Producto("P006", "Silla de oficina", "Hogar", 1, 78000, 0)],
                528000, 22500, 65715, 571215, "Tienda física", false),
            CrearVenta(8, new DateTime(2026, 10, 1, 10, 0, 0), "Cancelada", "C1008", "Andrés Chaves", "Alajuela", "Regular", "V016", "María Solís", "Heredia",
                [Producto("P003", "Cafetera", "Hogar", 1, 85000, 0)],
                85000, 0, 11050, 96050, "Sitio web", false),
            CrearVenta(9, new DateTime(2026, 10, 3, 15, 10, 0), "Completada", "C1009", "Daniela Vega", "San José", "Premium", "V017", "Jorge León", "Alajuela",
                [Producto("P006", "Silla de oficina", "Hogar", 1, 78000, 10)],
                78000, 7800, 9126, 79326, "Tienda física", true),
            CrearVenta(10, new DateTime(2026, 10, 5, 8, 50, 0), "Completada", "C1010", "Mateo Núñez", "Heredia", "Regular", "V015", "Carlos Méndez", "Escazú",
                [Producto("P002", "Mouse", "Tecnología", 2, 12000, 5), Producto("P004", "Café molido", "Alimentos", 2, 8500, 0)],
                41000, 1200, 5174, 44974, "Redes sociales", true)
        ];
    }

    private static Venta CrearVenta(
        int numero,
        DateTime fecha,
        string estado,
        string idCliente,
        string nombreCliente,
        string ciudad,
        string categoriaCliente,
        string idVendedor,
        string nombreVendedor,
        string sucursal,
        List<ProductoVenta> productos,
        decimal subtotal,
        decimal descuento,
        decimal impuesto,
        decimal total,
        string canal,
        bool promocion)
    {
        // Se usa este método para no repetir la creación de cada venta completa.
        return new Venta
        {
            Id = null,
            Fecha = fecha,
            Estado = estado,
            MetodoPago = numero % 2 == 0 ? "Tarjeta" : "Efectivo",
            Moneda = "CRC",
            Cliente = new Cliente
            {
                Identificacion = idCliente,
                Nombre = nombreCliente,
                Ciudad = ciudad,
                Provincia = ciudad switch
                {
                    "San José" => "San José",
                    "Heredia" => "Heredia",
                    "Cartago" => "Cartago",
                    _ => "Alajuela"
                },
                Categoria = categoriaCliente
            },
            Vendedor = new Vendedor
            {
                Identificacion = idVendedor,
                Nombre = nombreVendedor,
                Sucursal = sucursal,
                Region = "Central"
            },
            Productos = productos,
            Subtotal = subtotal,
            DescuentoTotal = descuento,
            Impuesto = impuesto,
            Total = total,
            CanalVenta = canal,
            TienePromocion = promocion
        };
    }

    private static ProductoVenta Producto(
        string id,
        string nombre,
        string categoria,
        int cantidad,
        decimal precio,
        decimal descuento)
    {
        return new ProductoVenta
        {
            IdProducto = id,
            Nombre = nombre,
            Categoria = categoria,
            Cantidad = cantidad,
            PrecioUnitario = precio,
            Descuento = descuento
        };
    }
}
