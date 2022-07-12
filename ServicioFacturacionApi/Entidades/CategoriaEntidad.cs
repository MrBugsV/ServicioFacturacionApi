using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{

    public class CategoriaEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public CategoriaEntidad()
        {
            Id = 0;
            Nombre = "";
        }

        public CategoriaEntidad(int idCategoria, string nombreCategoria)
        {
            Id = idCategoria;
            Nombre = nombreCategoria;
        }
    }
}