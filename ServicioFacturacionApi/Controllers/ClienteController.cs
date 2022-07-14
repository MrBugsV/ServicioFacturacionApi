using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ServicioFacturacionApi.Entidades;
using System.Web.Http.Cors;
using System.Data.Common;

namespace ServicioFacturacionApi.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClienteController : ApiController
    {
        #region Servicios REST
        // GET: api/Cliente
        public List<ClienteEntidad> Get()
        {
            return obtenerLista();
        }

        // GET: api/Cliente/5
        public ClienteEntidad Get(string id)
        {
            return obtenerCliente(id);
        }

        [HttpGet]
        [Route("api/Cliente/Cantidad/{cedula?}/{nombre?}")]
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
        [Route("api/Cliente/Paginacion/{pagina}/{cantidad}/{cedula?}/{nombre?}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<ClienteEntidad> GetClientesFiltro(int pagina, int cantidad, string cedula = "", string nombre = "")
        {
            if (cedula == null)
            {
                cedula = "";
            }
            return obtenerListaFiltrada(pagina, cantidad, cedula, nombre);
        }

        // POST: api/Cliente
        public bool Post([FromBody] ClienteEntidad cliente)
        {
            return insertarCliente(cliente);
        }

        // PUT: api/Cliente/
        public bool Put(ClienteEntidad cliente)
        {
            return actualizarCliente(cliente);
        }

        // DELETE: api/Cliente/5
        public bool Delete(string id)
        {
            return borrarCliente(id);
        }

        // DELETE: api/Cliente/
        public bool Delete(ClienteEntidad cliente)
        {
            return borrarCliente(cliente);
        }

        #endregion

        #region Metodos Base de Datos
        private List<ClienteEntidad> obtenerLista()
        {
            try
            {
                List<Cliente> clientes = new List<Cliente>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Cliente
                                    select p;
                    clientes = resultado.ToList();
                }

                List<ClienteEntidad> clienteEntidad = new List<ClienteEntidad>();

                foreach (Cliente item in clientes)
                {
                    clienteEntidad.Add(new ClienteEntidad(
                            item.Cedula,
                            item.Nombre,
                            item.Apellido,
                            item.Telefono,
                            item.Direccion,
                            item.Postal,
                            item.Correo));
                }


                return clienteEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private ClienteEntidad obtenerCliente(string id)
        {
            try
            {
                Cliente cliente;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.Cliente
                                    where p.Cedula == id
                                    select p;
                    cliente = (Cliente)resultado.FirstOrDefault();
                }

                if (cliente != null)
                {
                    ClienteEntidad clienteEntidad = new ClienteEntidad();

                    clienteEntidad = new ClienteEntidad(
                            cliente.Cedula,
                            cliente.Nombre,
                            cliente.Apellido,
                            cliente.Telefono,
                            cliente.Direccion,
                            cliente.Postal,
                            cliente.Correo);

                    return clienteEntidad;
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

        private List<ClienteEntidad> obtenerListaFiltrada(int pagina, int cantidad, string cedula, string nombre)
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
                    clientes = resultado.Skip(cantidad * (pagina - 1)).Take(cantidad).ToList();
                }

                List<ClienteEntidad> clienteEntidad = new List<ClienteEntidad>();

                foreach (Cliente item in clientes)
                {
                    clienteEntidad.Add(new ClienteEntidad(
                            item.Cedula,
                            item.Nombre,
                            item.Apellido,
                            item.Telefono,
                            item.Direccion,
                            item.Postal,
                            item.Correo));
                }


                return clienteEntidad;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool insertarCliente(ClienteEntidad cliente)
        {
            Cliente clienteBase = new Cliente();
            clienteBase.Cedula = cliente.Cedula;
            clienteBase.Nombre = cliente.Nombre;
            clienteBase.Apellido = cliente.Apellido;
            clienteBase.Telefono = cliente.Telefono;
            clienteBase.Postal = cliente.Postal;
            clienteBase.Direccion = cliente.Direccion;
            clienteBase.Correo = cliente.Correo;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Cliente.InsertOnSubmit(clienteBase);
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

        private bool actualizarCliente(ClienteEntidad cliente)
        {
            Cliente clienteBase = new Cliente();

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        var resultado = from p in dtc.Cliente
                                         where p.Cedula == cliente.Cedula
                                         select p;

                        clienteBase = (Cliente)resultado.First();

                        if (clienteBase.Nombre != cliente.Nombre)
                            clienteBase.Nombre = cliente.Nombre;

                        if (clienteBase.Apellido != cliente.Apellido)
                            clienteBase.Apellido = cliente.Apellido;

                        if (clienteBase.Telefono != cliente.Telefono)
                            clienteBase.Telefono = cliente.Telefono;

                        if (clienteBase.Postal != cliente.Postal)
                            clienteBase.Postal = cliente.Postal;

                        if (clienteBase.Direccion != cliente.Direccion)
                            clienteBase.Direccion = cliente.Direccion;

                        if (clienteBase.Correo != cliente.Correo)
                            clienteBase.Correo = cliente.Correo;

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

        private bool borrarCliente(ClienteEntidad cliente)
        {
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.Cliente.DeleteOnSubmit(dtc.Cliente.FirstOrDefault(x => x.Cedula == cliente.Cedula));

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

        private bool borrarCliente(string cedula)
        {
            return borrarCliente(obtenerCliente(cedula));
        }

        #endregion
    }

}
