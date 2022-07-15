using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using ServicioFacturacionApi.Entidades;

namespace ServicioFacturacionApi.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmpleadoController : ApiController
    {
        #region Servicios REST
        // GET: api/Empleado
        public List<EmpleadoEntidad> Get()
        {
            return obtenerLista();
        }

        // GET: api/Empleado/5
        public EmpleadoEntidad Get(string id)
        {
            return obtenerEmpleado(id);
        }

        [HttpGet]
        [Route("api/Empleado/Cantidad/{cedula?}/{nombre?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public int GetCount(string cedula = "", string nombre = "")
        {
            if (cedula == null)
            {
                cedula = "";
            }
            return obtenerCantidad(cedula, nombre);
        }

        [HttpGet]
        [Route("api/Empleado/Paginacion/{pagina}/{cantidad}/{cedula?}/{nombre?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<EmpleadoEntidad> GetEmpleadosFiltro(int pagina, int cantidad, string cedula = "", string nombre = "")
        {
            if (cedula == null)
            {
                cedula = "";
            }
            return obtenerListaFiltrada(pagina, cantidad, cedula, nombre);
        }

        // POST: api/Empleado
        public bool Post([FromBody] EmpleadoEntidad empleado)
        {
            return insertarEmpleado(empleado);
        }

        // PUT: api/Empleado/
        public bool Put(EmpleadoEntidad empleado)
        {
            return actualizarEmpleado(empleado);
        }

        // DELETE: api/Empleado/5
        public bool Delete(string id)
        {
            return borrarEmpleado(id);
        }

        // DELETE: api/Cliente/
        public bool Delete(EmpleadoEntidad empleado)
        {
            return borrarEmpleado(empleado);
        }

        #endregion

        #region Metodos Base de Datos
        private List<EmpleadoEntidad> obtenerLista()
        {
            try
            {
                List<Empleado> empleado = new List<Empleado>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Empleado
                                    select p;
                    empleado = resultado.ToList();
                }

                List<EmpleadoEntidad> empleadoEntidad = new List<EmpleadoEntidad>();

                foreach (Empleado item in empleado)
                {
                    empleadoEntidad.Add(new EmpleadoEntidad(
                            item.Cedula,
                            item.Nombre,
                            item.Apellido,
                            (bool)item.Admin));
                }


                return empleadoEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private EmpleadoEntidad obtenerEmpleado(string id)
        {
            try
            {
                Empleado empleado;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Empleado
                                    where p.Cedula == id
                                    select p;
                    empleado = (Empleado)resultado.FirstOrDefault();
                }

                if (empleado != null)
                {
                    EmpleadoEntidad empleadoEntidad = new EmpleadoEntidad();

                    empleadoEntidad = new EmpleadoEntidad(
                            empleado.Cedula,
                            empleado.Nombre,
                            empleado.Apellido,
                            (bool)empleado.Admin
                            );

                    return empleadoEntidad;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int obtenerCantidad(string cedula, string nombre)
        {
            try
            {
                List<Cliente> clientes = new List<Cliente>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Cliente
                                    where p.Cedula.Contains(cedula) && (p.Nombre + ' ' + p.Apellido).Contains(nombre)
                                    orderby p.Cedula, p.Nombre
                                    select p;
                    return resultado.Count();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<EmpleadoEntidad> obtenerListaFiltrada(int pagina, int cantidad, string cedula, string nombre)
        {
            try
            {
                List<Empleado> empleado = new List<Empleado>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Empleado
                                    where p.Cedula.Contains(cedula) && (p.Nombre + ' ' + p.Apellido).Contains(nombre)
                                    orderby p.Cedula, p.Nombre
                                    select p;
                    empleado = resultado.Skip(cantidad * (pagina - 1)).Take(cantidad).ToList();
                }

                List<EmpleadoEntidad> clienteEntidad = new List<EmpleadoEntidad>();

                foreach (Empleado item in empleado)
                {
                    clienteEntidad.Add(new EmpleadoEntidad(
                            item.Cedula,
                            item.Nombre,
                            item.Apellido,
                            (bool) item.Admin));
                }


                return clienteEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool insertarEmpleado(EmpleadoEntidad empleado)
        {
            Empleado empleadoBase = new Empleado();
            empleadoBase.Cedula = empleado.Cedula;
            empleadoBase.Nombre = empleado.Nombre;
            empleadoBase.Apellido = empleado.Apellido;
            empleadoBase.Admin = empleado.Admin;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Empleado.InsertOnSubmit(empleadoBase);
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

        private bool actualizarEmpleado(EmpleadoEntidad empleado)
        {
            Empleado empleadoBase = new Empleado();

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        var resultado = from p in dtc.Empleado
                                        where p.Cedula == empleado.Cedula
                                        select p;

                        empleadoBase = (Empleado)resultado.First();

                        if (empleadoBase.Nombre != empleado.Nombre)
                            empleadoBase.Nombre = empleado.Nombre;

                        if (empleadoBase.Apellido != empleado.Apellido)
                            empleadoBase.Apellido = empleado.Apellido;

                        if (empleadoBase.Admin != empleado.Admin)
                            empleadoBase.Admin = empleado.Admin;

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

        private bool borrarEmpleado(EmpleadoEntidad empleado)
        {
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Empleado.DeleteOnSubmit(dtc.Empleado.FirstOrDefault(x => x.Cedula == empleado.Cedula));

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

        private bool borrarEmpleado(string cedula)
        {
            return borrarEmpleado(obtenerEmpleado(cedula));
        }

        #endregion
    }
}
