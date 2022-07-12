using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class FacturaDetalleEntidad
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public FacturaDetalleEntidad()
        {
            Id = 0;
            Cantidad = 0;
            Subtotal = 0;
            IdFactura = 0;
            IdProducto = 0;
        }

        public FacturaDetalleEntidad(int id, int cantidad, decimal subtotal, int idFactura, int idProducto)
        {
            Id = id;
            Cantidad = cantidad;
            Subtotal = subtotal;
            IdFactura = idFactura;
            IdProducto = idProducto;
        }
    }

    public class vFacturasDetalleEntidad
    {
        public int IdFacturaCabecera { get; set; }
        public int IdFacturaDetalle { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public string NombreCategoria { get; set; }
        public vFacturasDetalleEntidad()
        {
            IdFacturaCabecera = 0;
            IdFacturaDetalle = 0;
            IdProducto = 0;
            NombreProducto = "";
            Cantidad = 0;
            Precio = 0;
            Subtotal = 0;
            NombreCategoria = "";
        }

        public vFacturasDetalleEntidad(int idFacturaCabecera, int idFacturaDetalle, int idProducto, string nombreProducto, int cantidad, decimal precio, decimal subtotal, string nombreCategoria)
        {
            IdFacturaCabecera = idFacturaCabecera;
            IdFacturaDetalle = idFacturaDetalle;
            IdProducto = idProducto;
            NombreProducto = nombreProducto;
            Cantidad = cantidad;
            Precio = precio;
            Subtotal = subtotal;
            NombreCategoria = nombreCategoria;
        }
    }
}