using MongoDB.Bson;
using MongoDB.Driver;

namespace AnalisisVentasApi.Services;

// Obtiene y guarda ventas; usa datos de ejemplo cuando no se configura Atlas.
public class VentasRepository
{
    private readonly IMongoCollection<Venta>? _coleccion;
    private readonly List<Venta> _ventasEjemplo;

    public VentasRepository(IConfiguration configuration, IMongoClient cliente)
    {
        var cadenaConexion = configuration["MongoDB:ConnectionString"];
        var nombreBaseDatos = configuration["MongoDB:Database"] ?? "AnalisisVentas";
        var nombreColeccion = configuration["MongoDB:Collection"] ?? "Ventas";

        if (string.IsNullOrWhiteSpace(cadenaConexion))
        {
            // Así se puede probar la API sin configurar una base de datos.
            _ventasEjemplo = DatosEjemplo.CrearVentas();
            Modo = "Demostración (datos en memoria)";
            return;
        }

        _coleccion = cliente.GetDatabase(nombreBaseDatos).GetCollection<Venta>(nombreColeccion);
        _ventasEjemplo = [];
        Modo = "MongoDB Atlas";
    }

    public string Modo { get; }

    public async Task<List<Venta>> ObtenerTodasAsync()
    {
        return _coleccion is null
            ? _ventasEjemplo
            : await _coleccion.Find(FilterDefinition<Venta>.Empty).ToListAsync();
    }

    public async Task<Venta?> ObtenerPorIdAsync(string id)
    {
        if (_coleccion is null)
        {
            return _ventasEjemplo.FirstOrDefault(v => v.Id == id);
        }

        if (!ObjectId.TryParse(id, out _))
        {
            return null;
        }

        return await _coleccion.Find(v => v.Id == id).FirstOrDefaultAsync();
    }

    public async Task GuardarAsync(Venta venta)
    {
        if (_coleccion is null)
        {
            venta.Id = Guid.NewGuid().ToString();
            _ventasEjemplo.Add(venta);
            return;
        }

        await _coleccion.InsertOneAsync(venta);
    }

    public async Task<bool> CargarEjemplosSiEstaVacioAsync()
    {
        var ejemplos = DatosEjemplo.CrearVentas();
        if (_coleccion is null)
        {
            if (_ventasEjemplo.Count > 0)
            {
                return false;
            }

            _ventasEjemplo.AddRange(ejemplos);
            return true;
        }

        // Los ejemplos se cargan solo cuando la colección todavía está vacía.
        var cantidad = await _coleccion.CountDocumentsAsync(FilterDefinition<Venta>.Empty);
        if (cantidad > 0)
        {
            return false;
        }

        await _coleccion.InsertManyAsync(ejemplos);
        return true;
    }
}
