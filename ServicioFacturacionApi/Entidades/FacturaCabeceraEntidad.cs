using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class FacturaCabeceraEntidad
    {
        public int Id { get; set; }
        public string IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string UsuarioId { get; set; }
        public bool Anulado { get; set; }
        public List<FacturaDetalleEntidad> DetallesFactura { get; set; }

        public FacturaCabeceraEntidad()
        {
            Id = 0;
            IdCliente = "";
            Fecha = new DateTime();
            Subtotal = 0;
            Iva = 0;
            Total = 0;
            UsuarioId = "";
            Anulado = false;
            DetallesFactura = new List<FacturaDetalleEntidad>();
        }

        public FacturaCabeceraEntidad(int id, string idCliente, DateTime fecha, decimal subtotal, decimal iva, decimal total, string usuarioId, bool anulado)
        {
            Id = id;
            IdCliente = idCliente;
            Fecha = fecha;
            Subtotal = subtotal;
            Iva = iva;
            Total = total;
            UsuarioId = usuarioId;
            Anulado = anulado;
        }
    }

    public class vFacturasCabeceraEntidad
    {
        public int Id { get; set; }
        public string IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public bool Anulado { get; set; }
        public List<vFacturasDetalleEntidad> DetallesFactura { get; set; }

        public vFacturasCabeceraEntidad()
        {
            Id = 0;
            IdCliente = "";
            Nombre = "";
            Apellido = "";
            Direccion = "";
            Telefono = "";
            Fecha = new DateTime();
            Subtotal = 0;
            Iva = 0;
            Total = 0;
            Anulado = false;
            DetallesFactura = new List<vFacturasDetalleEntidad>();
        }

        public vFacturasCabeceraEntidad(int id, string idCliente, string nombre, string apellido, string direccion, string telefono, DateTime fecha, decimal subtotal, decimal iva, decimal total, bool anulado, List<vFacturasDetalleEntidad> detallesFactura)
        {
            Id = id;
            IdCliente = idCliente;
            Nombre = nombre;
            Apellido = apellido;
            Direccion = direccion;
            Telefono = telefono;
            Fecha = fecha;
            Subtotal = subtotal;
            Iva = iva;
            Total = total;
            Anulado = anulado;
            DetallesFactura = detallesFactura;
        }
    }
}