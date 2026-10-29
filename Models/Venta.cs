using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalisisVentasApi;

// Estos modelos representan el documento de venta y sus datos anidados en MongoDB.
public class Venta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("fecha")]
    public DateTime Fecha { get; set; }

    [BsonElement("estado")]
    public string Estado { get; set; } = "";

    [BsonElement("metodoPago")]
    public string MetodoPago { get; set; } = "";

    [BsonElement("moneda")]
    public string Moneda { get; set; } = "CRC";

    [BsonElement("cliente")]
    public Cliente Cliente { get; set; } = new();

    [BsonElement("vendedor")]
    public Vendedor Vendedor { get; set; } = new();

    [BsonElement("productos")]
    public List<ProductoVenta> Productos { get; set; } = [];

    [BsonElement("subtotal")]
    public decimal Subtotal { get; set; }

    [BsonElement("descuentoTotal")]
    public decimal DescuentoTotal { get; set; }

    [BsonElement("impuesto")]
    public decimal Impuesto { get; set; }

    [BsonElement("total")]
    public decimal Total { get; set; }

    [BsonElement("canalVenta")]
    public string CanalVenta { get; set; } = "";

    [BsonElement("tienePromocion")]
    public bool TienePromocion { get; set; }
}

public class Cliente
{
    [BsonElement("identificacion")]
    public string Identificacion { get; set; } = "";

    [BsonElement("nombre")]
    public string Nombre { get; set; } = "";

    [BsonElement("ciudad")]
    public string Ciudad { get; set; } = "";

    [BsonElement("provincia")]
    public string Provincia { get; set; } = "";

    [BsonElement("categoria")]
    public string Categoria { get; set; } = "";
}

public class Vendedor
{
    [BsonElement("identificacion")]
    public string Identificacion { get; set; } = "";

    [BsonElement("nombre")]
    public string Nombre { get; set; } = "";

    [BsonElement("sucursal")]
    public string Sucursal { get; set; } = "";

    [BsonElement("region")]
    public string Region { get; set; } = "";
}

public class ProductoVenta
{
    [BsonElement("idProducto")]
    public string IdProducto { get; set; } = "";

    [BsonElement("nombre")]
    public string Nombre { get; set; } = "";

    [BsonElement("categoria")]
    public string Categoria { get; set; } = "";

    [BsonElement("cantidad")]
    public int Cantidad { get; set; }

    [BsonElement("precioUnitario")]
    public decimal PrecioUnitario { get; set; }

    [BsonElement("descuento")]
    public decimal Descuento { get; set; }
}
