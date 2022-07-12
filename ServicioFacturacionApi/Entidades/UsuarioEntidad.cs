using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class vUsuarioEntidad
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public bool Admin { get; set; }

        public vUsuarioEntidad()
        {

        }

        public vUsuarioEntidad(string cedula, string nombre, string apellido, string contraseña, bool admin)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Contraseña = contraseña;
            Admin = admin;
        }
    }
    public class UsuarioEntidad
    {
        public string Cedula { get; set; }
        public string Contraseña { get; set; }
        public bool Admin { get; set; }

        public UsuarioEntidad()
        {
        }

        public UsuarioEntidad(string cedula, string contraseña, bool admin)
        {
            Cedula = cedula;
            Contraseña = contraseña;
            Admin = admin;
        }
    }
}