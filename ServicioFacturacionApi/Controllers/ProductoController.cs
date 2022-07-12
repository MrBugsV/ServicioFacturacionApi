using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ServicioFacturacionApi.Entidades;
using System.Web.Http.Cors;
using System.Data.Common;

namespace ServicioFacturacionApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductoController : ApiController
    {
        #region Servicios REST
        // GET api/Producto
        public List<vProductoEntidad> Get()
        {
            return obtenerLista();
        }

        // GET api/Producto/5
        public vProductoEntidad Get(int id)
        {
            return obtenerProducto(id);
        }

        [HttpGet]
        [Route("api/Producto/Cantidad/{producto?}/{categoria?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public int GetCount(string producto = "", string categoria = "")
        {
            if (producto == null)
            {
                producto = "";
            }
            return obtenerCantidad(producto, categoria);
        }


        [HttpGet]
        [Route("api/Producto/Paginacion/{pagina}/{cantidad}/{producto?}/{categoria?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<vProductoEntidad> GetProductoFiltro(int pagina, int cantidad, string producto = "", string categoria = "")
        {
            if (producto == null)
            {
                producto = "";
            }
            return obtenerListaFiltrada(pagina, cantidad, producto, categoria);
        }

        // POST api/Producto
        public int Post([FromBody] vProductoEntidad producto)
        {
            return insertarProducto(producto);
        }

        // PUT api/Producto/5
        public bool Put([FromBody] vProductoEntidad producto)
        {
            return actualizarProducto(producto);
        }

        // DELETE api/Producto/5
        public bool Delete(int id)
        {
            return borrarProducto(id);
        }

        // DELETE api/Producto
        public bool Delete(vProductoEntidad producto)
        {
            return borrarProducto(producto);
        }

        #endregion

        #region Metodos Bases de Datos

        private List<vProductoEntidad> obtenerLista()
        {
            try
            {
                List<vProductos> productos = new List<vProductos>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vProductos
                                    select p;
                    productos = resultado.ToList();
                }

                List<vProductoEntidad> productoEntidad = new List<vProductoEntidad>();

                foreach (vProductos item in productos)
                {
                    productoEntidad.Add(new vProductoEntidad(
                        item.Id,
                        item.NombreProducto,
                        (decimal)item.Precio,
                        (decimal)item.Stock,
                        item.Imagen,
                        (int)item.Categoria_Id,
                        item.NombreCategoria));
                }


                return productoEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private vProductoEntidad obtenerProducto(int id)
        {
            try
            {
                vProductos producto = new vProductos();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vProductos
                                    where p.Id == id
                                    select p;
                    producto = (vProductos)resultado.FirstOrDefault();
                }

                if (producto != null)
                {
                    vProductoEntidad productoEntidad = new vProductoEntidad();
                    productoEntidad = new vProductoEntidad(
                        producto.Id,
                        producto.NombreProducto,
                        (decimal)producto.Precio,
                        (decimal)producto.Stock,
                        producto.Imagen,
                        (int)producto.Categoria_Id,
                        producto.NombreCategoria);

                    return productoEntidad;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int obtenerCantidad(string producto, string categoria)
        {
            try
            {
                List<vClientes> clientes = new List<vClientes>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vProductos
                                    where p.NombreProducto.Contains(producto) && p.NombreCategoria.Contains(categoria)
                                    orderby p.NombreCategoria, p.NombreProducto
                                    select p;
                    return resultado.Count();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<vProductoEntidad> obtenerListaFiltrada(int pagina, int cantidad, string producto, string categoria)
        {
            try
            {
                List<vProductos> productos = new List<vProductos>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vProductos
                                    where p.NombreProducto.Contains(producto) && p.NombreCategoria.Contains(categoria)
                                    orderby p.NombreCategoria, p.NombreProducto
                                    select p;
                    productos = resultado.Skip(cantidad * (pagina - 1)).Take(cantidad).ToList();
                }

                List<vProductoEntidad> productoEntidad = new List<vProductoEntidad>();

                foreach (vProductos item in productos)
                {
                    productoEntidad.Add(new vProductoEntidad(
                        item.Id,
                        item.NombreProducto,
                        (decimal)item.Precio,
                        (decimal)item.Stock,
                        item.Imagen,
                        (int)item.Categoria_Id,
                        item.NombreCategoria));
                }


                return productoEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int insertarProducto(vProductoEntidad producto)
        {
            Producto productoBase = new Producto();
            productoBase.Nombre = producto.NombreProducto;
            productoBase.Precio = producto.Precio;
            productoBase.Stock = producto.Stock;
            productoBase.Categoria_Id = producto.IdCategoria;
            productoBase.Imagen = producto.Imagen;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Producto.InsertOnSubmit(productoBase);
                        dtc.SubmitChanges();
                        tscope.Commit();
                    }
                    catch (Exception)
                    {
                        tscope.Rollback();
                        dtc.Connection.Close();
                        throw;
                    }
                }
                dtc.Connection.Close();
                return productoBase.Id;
            }

        }

        private bool actualizarProducto(vProductoEntidad producto)
        {
            Producto productoBase;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {

                        var resultado1 = from p in dtc.Producto
                                         where p.Id == producto.Id
                                         select p;

                        productoBase = (Producto)resultado1.First();

                        productoBase.Nombre = producto.NombreProducto;
                        productoBase.Precio = producto.Precio;
                        productoBase.Stock = producto.Stock;
                        productoBase.Categoria_Id = producto.IdCategoria;
                        productoBase.Imagen = producto.Imagen;
                        dtc.SubmitChanges();
                        tscope.Commit();
                    }
                    catch (Exception)
                    {
                        tscope.Rollback();
                        dtc.Connection.Close();
                        throw;
                    }
                }
                dtc.Connection.Close();
                return true;
            }
        }

        private bool borrarProducto(vProductoEntidad producto)
        {
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Producto.DeleteOnSubmit(dtc.Producto.FirstOrDefault(x => x.Id == producto.Id));

                        dtc.SubmitChanges();
                        tscope.Commit();
                    }
                    catch (Exception)
                    {
                        tscope.Rollback();
                        dtc.Connection.Close();
                        throw;
                    }
                }
                dtc.Connection.Close();
                return true;
            }
        }

        #endregion

        #region Metodos

        private bool borrarProducto(int id)
        {
            return borrarProducto(obtenerProducto(id));
        }

        #endregion

    }
}