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
        public string IdEmpleado { get; set; }
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
            IdEmpleado = "";
            Fecha = new DateTime();
            Subtotal = 0;
            Iva = 0;
            Total = 0;
            UsuarioId = "";
            Anulado = false;
            DetallesFactura = new List<FacturaDetalleEntidad>();
        }

        public FacturaCabeceraEntidad(int id, string idCliente, string idEmpleado, DateTime fecha, decimal subtotal, decimal iva, decimal total, string usuarioId, bool anulado, List<FacturaDetalleEntidad> detallesFactura)
        {
            Id = id;
            IdCliente = idCliente;
            IdEmpleado = idEmpleado;
            Fecha = fecha;
            Subtotal = subtotal;
            Iva = iva;
            Total = total;
            UsuarioId = usuarioId;
            Anulado = anulado;
            DetallesFactura = detallesFactura;
        }
    }

    public class vFacturasCabeceraEntidad
    {
        public int Id { get; set; }
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
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
            NombreCliente = "";
            ApellidoCliente = "";
            Direccion = "";
            Telefono = "";
            Fecha = new DateTime();
            Subtotal = 0;
            Iva = 0;
            Total = 0;
            Anulado = false;
            DetallesFactura = new List<vFacturasDetalleEntidad>();
        }

        public vFacturasCabeceraEntidad(int id, string idCliente, string nombreCliente, string apellidoCliente, string direccion, string telefono, string idEmpleado, string nombreEmpleado, string apellidoEmpleado, DateTime fecha, decimal subtotal, decimal iva, decimal total, bool anulado, List<vFacturasDetalleEntidad> detallesFactura)
        {
            Id = id;
            IdCliente = idCliente;
            NombreCliente = nombreCliente;
            ApellidoCliente = apellidoCliente;
            Direccion = direccion;
            Telefono = telefono;
            IdEmpleado = idEmpleado;
            NombreEmpleado = nombreEmpleado;
            ApellidoEmpleado = apellidoEmpleado;
            Fecha = fecha;
            Subtotal = subtotal;
            Iva = iva;
            Total = total;
            Anulado = anulado;
            DetallesFactura = detallesFactura;
        }
    }
}