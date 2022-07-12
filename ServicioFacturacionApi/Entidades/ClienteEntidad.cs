using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{

    public class vClienteEntidad
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Postal { get; set; }
        public string Correo { get; set; }

        public vClienteEntidad()
        {
            Cedula = "";
            Nombre = "";
            Apellido = "";
            Telefono = "";
            Direccion = "";
            Postal = "";
            Correo = "";
        }

        public vClienteEntidad(string cedula, string nombre, string apellido, string telefono, string direccion, string postal, string correo)
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
    public class PersonaEntidad
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public PersonaEntidad()
        {
            Cedula = "";
            Nombre = "";
            Apellido = "";
        }

        public PersonaEntidad(string cedula, string nombre, string apellido)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
        }
    }

    public class ClienteEntidad
    {
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Postal { get; set; }
        public string Correo { get; set; }

        public ClienteEntidad()
        {

            Cedula = "";
            Telefono = "";
            Direccion = "";
            Postal = "";
            Correo = "";
        }

        public ClienteEntidad(string cedula, string telefono, string direccion, string postal, string correo)
        {
            Cedula = cedula;
            Telefono = telefono;
            Direccion = direccion;
            Postal = postal;
            Correo = correo;
        }
    }

}