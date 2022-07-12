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
    public class FacturaController : ApiController
    {
        #region Servicios REST
        // GET: api/Factura
        public List<vFacturasCabeceraEntidad> Get()
        {
            return obtenerLista();
        }

        // GET: api/Factura/5
        public vFacturasCabeceraEntidad Get(int id)
        {
            return obtenerFacturaCabecera(id);
        }

        // GET: api/Factura
        [HttpGet]
        [Route("api/Factura/Id")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public int GetNum()
        {
            return obtenerIdFactura();
        }

        [HttpGet]
        [Route("api/Factura/Cantidad/{facturaId?}/{cedula?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public int GetCount(string facturaId = "", string cedula = "")
        {
            if (facturaId == null)
            {
                facturaId = "";
            }
            return obtenerCantidad(facturaId, cedula);
        }


        [HttpGet]
        [Route("api/Factura/Paginacion/{pagina}/{cantidad}/{facturaId?}/{cedula?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<vFacturasCabeceraEntidad> GetClientesFiltro(int pagina, int cantidad, string facturaId = "", string cedula = "")
        {
            if (facturaId == null)
            {
                facturaId = "";
            }
            return obtenerListaFiltrada(pagina, cantidad, facturaId, cedula);
        }

        [HttpGet]
        [Route("api/Factura/Fecha")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public DateTime GetFecha()
        {
            return DateTime.Now;
        }

        // POST: api/FacturaCabecera
        public int Post([FromBody] vFacturasCabeceraEntidad facturaCabecera)
        {
            return insertarFacturaCabecera(facturaCabecera);
        }

        // PUT: api/FacturaCabecera/5
        public bool Put([FromBody] vFacturasCabeceraEntidad facturaCabecera)
        {
            return actualizarFacturaCabecera(facturaCabecera);
        }

        //// DELETE: api/FacturaDetalle/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        #region Metodos Base de Datos
        private List<vFacturasCabeceraEntidad> obtenerLista()
        {
            try
            {
                List<vFacturasCabecera> facturasCabecera;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vFacturasCabecera
                                    select p;
                    facturasCabecera = resultado.ToList();
                }

                List<vFacturasCabeceraEntidad> facturaCabeceraEntidad = new List<vFacturasCabeceraEntidad>();

                foreach (vFacturasCabecera item in facturasCabecera)
                {
                    facturaCabeceraEntidad.Add(new vFacturasCabeceraEntidad(
                            item.Id,
                            item.Cliente_Id,
                            item.Nombre,
                            item.Apellido,
                            item.Direccion,
                            item.Telefono,
                            (DateTime)item.Fecha,
                            (decimal)item.Subtotal,
                            (decimal)item.Iva,
                            (decimal)item.Total,
                            (bool)item.Anulado,
                            null));
                }

                return facturaCabeceraEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private vFacturasCabeceraEntidad obtenerFacturaCabecera(int id)
        {
            try
            {
                vFacturasCabecera facturaCabecera = new vFacturasCabecera();
                List<vFacturasDetalle> facturaDetalles = new List<vFacturasDetalle>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vFacturasCabecera
                                    where p.Id == id
                                    select p;
                    facturaCabecera = (vFacturasCabecera)resultado.FirstOrDefault();

                    var resultado1 = from p in dtc.vFacturasDetalle
                                     where p.IdFacturaCabecera == id
                                     select p;
                    facturaDetalles = (List<vFacturasDetalle>)resultado1.ToList();
                }

                if (facturaCabecera != null)
                {
                    vFacturasCabeceraEntidad facturaCabeceraEntidad = new vFacturasCabeceraEntidad();
                    List<vFacturasDetalleEntidad> facturasDetalleEntidad = new List<vFacturasDetalleEntidad>();

                    foreach (vFacturasDetalle detalle in facturaDetalles)
                    {
                        facturasDetalleEntidad.Add(
                            new vFacturasDetalleEntidad(
                                detalle.IdFacturaCabecera,
                                detalle.IdFacturaDetalle,
                                detalle.IdProducto,
                                detalle.NombreProducto,
                                (int)detalle.Cantidad,
                                (decimal)detalle.Precio,
                                (decimal)detalle.Subtotal,
                                detalle.NombreCategoria
                                )
                            );
                    }

                    facturaCabeceraEntidad = new vFacturasCabeceraEntidad(
                        facturaCabecera.Id,
                        facturaCabecera.Cliente_Id,
                        facturaCabecera.Nombre,
                        facturaCabecera.Apellido,
                        facturaCabecera.Direccion,
                        facturaCabecera.Telefono,
                        (DateTime)facturaCabecera.Fecha,
                        (decimal)facturaCabecera.Subtotal,
                        (decimal)facturaCabecera.Iva,
                        (decimal)facturaCabecera.Total,
                        (bool)facturaCabecera.Anulado,
                        facturasDetalleEntidad);

                    return facturaCabeceraEntidad;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int obtenerIdFactura()
        {

            try
            {
                vFacturasCabecera factura;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vFacturasCabecera
                                    select p;
                    factura = resultado.ToList().LastOrDefault();
                }

                if (factura != null)
                {
                    return factura.Id + 1;
                }

                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int obtenerCantidad(string facturaId, string cedula)
        {
            try
            {
                List<vClientes> clientes = new List<vClientes>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vFacturasCabecera
                                    where p.Id.ToString().Contains(facturaId) && p.Cliente_Id.Contains(cedula)
                                    orderby p.Id, p.Cliente_Id
                                    select p;
                    return resultado.Count();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<vFacturasCabeceraEntidad> obtenerListaFiltrada(int pagina, int cantidad, string facturaId, string cedula)
        {
            try
            {
                List<vFacturasCabecera> clientes = new List<vFacturasCabecera>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vFacturasCabecera
                                    where p.Id.ToString().Contains(facturaId) && p.Cliente_Id.Contains(cedula)
                                    orderby p.Id, p.Cliente_Id
                                    select p;
                    clientes = resultado.Skip(cantidad * (pagina - 1)).Take(cantidad).ToList();
                }

                List<vFacturasCabeceraEntidad> facturaEntidad = new List<vFacturasCabeceraEntidad>();

                foreach (vFacturasCabecera item in clientes)
                {
                    facturaEntidad.Add(new vFacturasCabeceraEntidad(
                            item.Id,
                            item.Cliente_Id,
                            item.Nombre,
                            item.Apellido,
                            item.Direccion,
                            item.Telefono,
                            (DateTime)item.Fecha,
                            (decimal)item.Subtotal,
                            (decimal)item.Iva,
                            (decimal)item.Total,
                            (bool)item.Anulado,
                            null));
                }


                return facturaEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int insertarFacturaCabecera(vFacturasCabeceraEntidad facturaCabecera)
        {
            FacturaCabecera facturaCabeceraBase = new FacturaCabecera();
            facturaCabeceraBase.Cliente_Id = facturaCabecera.IdCliente;
            facturaCabeceraBase.Fecha = facturaCabecera.Fecha;
            facturaCabeceraBase.Subtotal = facturaCabecera.Subtotal;
            facturaCabeceraBase.Iva = facturaCabecera.Iva;
            facturaCabeceraBase.Total = facturaCabecera.Total;
            facturaCabeceraBase.Anulado = false;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.FacturaCabecera.InsertOnSubmit(facturaCabeceraBase);
                        dtc.SubmitChanges();
                        foreach (vFacturasDetalleEntidad detalle in facturaCabecera.DetallesFactura)
                        {
                            detalle.IdFacturaCabecera = facturaCabeceraBase.Id;
                            insertarFacturaDetalle(detalle, dtc);
                        }

                        tscope.Commit();
                    }
                    catch (Exception)
                    {
                        tscope.Rollback();
                        dtc.ExecuteCommand("DBCC CHECKIDENT('FacturaCabecera', RESEED, " + dtc.FacturaCabecera.ToList().Last().Id + ")");
                        dtc.SubmitChanges();
                        dtc.Connection.Close();
                        throw;
                    }
                }
                dtc.Connection.Close();
                return facturaCabeceraBase.Id;
            }
        }

        private bool insertarFacturaDetalle(vFacturasDetalleEntidad facturaDetalle, FacturasDataContext dtc)
        {
            FacturaDetalle facturaDetalleBase = new FacturaDetalle();
            facturaDetalleBase.Producto_Id = facturaDetalle.IdProducto;
            facturaDetalleBase.Factura_Id = facturaDetalle.IdFacturaCabecera;
            facturaDetalleBase.Cantidad = facturaDetalle.Cantidad;
            facturaDetalleBase.Subtotal = facturaDetalle.Subtotal;

            try
            {
                dtc.FacturaDetalle.InsertOnSubmit(facturaDetalleBase);
                dtc.SubmitChanges();
                facturaDetalleBase.Producto.Stock -= facturaDetalleBase.Cantidad;
                dtc.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        private bool actualizarFacturaCabecera(vFacturasCabeceraEntidad facturaCabecera)
        {
            FacturaCabecera facturaCabeceraBase;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();

                var resultado1 = from p in dtc.FacturaCabecera
                                 where p.Id == facturaCabecera.Id
                                 select p;

                facturaCabeceraBase = (FacturaCabecera)resultado1.First();

                if ((bool)facturaCabeceraBase.Anulado)
                {
                    return false;
                }

                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        facturaCabeceraBase.Anulado = facturaCabecera.Anulado;

                        foreach (FacturaDetalle detalle in facturaCabeceraBase.FacturaDetalle)
                        {
                            detalle.Producto.Stock += detalle.Cantidad;
                        }
                    }
                    catch (Exception)
                    {
                        tscope.Rollback();
                        dtc.Connection.Close();
                        throw;
                    }

                }
                dtc.Connection.Close();
                dtc.SubmitChanges();
            }
            return true;

        }

        #endregion
    }
}