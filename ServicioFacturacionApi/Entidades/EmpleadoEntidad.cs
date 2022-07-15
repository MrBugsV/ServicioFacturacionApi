using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class EmpleadoEntidad
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Admin { get; set; }

        public EmpleadoEntidad()
        {

        }

        public EmpleadoEntidad(string cedula, string nombre, string apellido, bool admin)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Admin = admin;
        }
    }
}