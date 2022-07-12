using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class vProductoEntidad
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public decimal Stock { get; set; }
        public Binary Imagen { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }

        public vProductoEntidad()
        {
            Id = 0;
            NombreProducto = "";
            Precio = 0;
            Stock = 0;
            Imagen = new Binary(new byte[0]);
            IdCategoria = 9;
            NombreCategoria = "";
        }

        public vProductoEntidad(int id, string nombreProducto, decimal precio, decimal stock, Binary imagen, int idCategoria, string nombreCategoria)
        {
            Id = id;
            NombreProducto = nombreProducto;
            Precio = precio;
            Stock = stock;
            Imagen = imagen;
            IdCategoria = idCategoria;
            NombreCategoria = nombreCategoria;
        }
    }
    public class ProductoEntidad
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public decimal Stock { get; set; }
        public Binary Imagen { get; set; }
        public int IdCategoria { get; set; }

        public ProductoEntidad()
        {
            Id = 0;
            NombreProducto = "";
            Precio = 0;
            Stock = 0;
            Imagen = new Binary(new byte[0]);
            IdCategoria = 0;
        }

        public ProductoEntidad(int id, string nombreProducto, decimal precio, decimal stock, Binary imagen, int idCategoria)
        {
            Id = id;
            NombreProducto = nombreProducto;
            Precio = precio;
            Stock = stock;
            Imagen = imagen;
            IdCategoria = idCategoria;
        }
    }
}