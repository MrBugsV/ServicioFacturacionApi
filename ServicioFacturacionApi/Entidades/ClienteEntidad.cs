using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class ClienteEntidad
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Postal { get; set; }
        public string Correo { get; set; }

        public ClienteEntidad()
        {
            Cedula = "";
            Nombre = "";
            Apellido = "";
            Telefono = "";
            Direccion = "";
            Postal = "";
            Correo = "";
        }

        public ClienteEntidad(string cedula, string nombre, string apellido, string telefono, string direccion, string postal, string correo)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Direccion = direccion;
            Postal = postal;
            Correo = correo;
        }
    }

}