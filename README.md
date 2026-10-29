# Análisis de ventas

Este proyecto es una API para guardar y analizar ventas. Permite ver un resumen de los resultados en una página sencilla y consultar la información en formato JSON. Se hizo como práctica para trabajar con C#, LINQ y MongoDB Atlas.

## Para empezar

Necesitas tener instalado .NET 8 o una versión más reciente. Abre PowerShell en esta carpeta y ejecuta:

```powershell
dotnet run
```

La consola muestra la dirección para abrir el panel de ventas en el navegador. 

## Conectar MongoDB Atlas

La conexión se configura con variables de entorno en PowerShell.

```powershell
Invoke-RestMethod -Method Post -Uri http://localhost:5169/api/ventas/cargar-ejemplo
```

La dirección puede cambiar al ejecutar el proyecto; usa la que aparezca en la consola. El reporte en formato JSON está en `/api/analisis/reporte`.

## Qué se puede consultar

- El resumen general de las ventas.
- Los productos más vendidos y sus ingresos.
- Los ingresos por categoría.
- Las ventas por ciudad.
- Los resultados por vendedor.

También se incluye una colección de Postman en `postman/AnalisisVentas.postman_collection.json` para probar las consultas.

## Archivos principales

- `Models`: la información de cada venta, cliente, vendedor y producto.
- `Endpoints`: las direcciones que se pueden consultar.
- `Services`: el acceso a los datos y los cálculos con LINQ.
- `wwwroot/index.html`: el panel que muestra los resultados.

## Repositorio del proyecto

https://github.com/Cristhian1607/SistemaVentasAC2P6CAD
