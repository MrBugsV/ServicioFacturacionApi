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
    public class ClienteController : ApiController
    {
        #region Servicios REST
        // GET: api/Cliente
        public List<vClienteEntidad> Get()
        {
            return obtenerLista();
        }

        // GET: api/Cliente/5
        public vClienteEntidad Get(string id)
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
        public List<vClienteEntidad> GetClientesFiltro(int pagina, int cantidad, string cedula = "", string nombre = "")
        {
            if (cedula == null)
            {
                cedula = "";
            }
            return obtenerListaFiltrada(pagina, cantidad, cedula, nombre);
        }

        // POST: api/Cliente
        public bool Post([FromBody] vClienteEntidad cliente)
        {
            return insertarCliente(cliente);
        }

        // PUT: api/Cliente/
        public bool Put(vClienteEntidad cliente)
        {
            return actualizarCliente(cliente);
        }

        // DELETE: api/Cliente/5
        public bool Delete(string id)
        {
            return borrarCliente(id);
        }

        // DELETE: api/Cliente/
        public bool Delete(vClienteEntidad cliente)
        {
            return borrarCliente(cliente);
        }

        #endregion

        #region Metodos Base de Datos
        private List<vClienteEntidad> obtenerLista()
        {
            try
            {
                List<vClientes> clientes = new List<vClientes>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vClientes
                                    select p;
                    clientes = resultado.ToList();
                }

                List<vClienteEntidad> clienteEntidad = new List<vClienteEntidad>();

                foreach (vClientes item in clientes)
                {
                    clienteEntidad.Add(new vClienteEntidad(
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

        private vClienteEntidad obtenerCliente(string id)
        {
            try
            {
                vClientes cliente;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vClientes
                                    where p.Cedula == id
                                    select p;
                    cliente = (vClientes)resultado.FirstOrDefault();
                }

                if (cliente != null)
                {
                    vClienteEntidad clienteEntidad = new vClienteEntidad();

                    clienteEntidad = new vClienteEntidad(
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
                List<vClientes> clientes = new List<vClientes>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vClientes
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

        private List<vClienteEntidad> obtenerListaFiltrada(int pagina, int cantidad, string cedula, string nombre)
        {
            try
            {
                List<vClientes> clientes = new List<vClientes>();
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    var resultado = from p in dtc.vClientes
                                    where p.Cedula.Contains(cedula) && (p.Nombre + ' ' + p.Apellido).Contains(nombre)
                                    orderby p.Cedula, p.Nombre
                                    select p;
                    clientes = resultado.Skip(cantidad * (pagina - 1)).Take(cantidad).ToList();
                }

                List<vClienteEntidad> clienteEntidad = new List<vClienteEntidad>();

                foreach (vClientes item in clientes)
                {
                    clienteEntidad.Add(new vClienteEntidad(
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

        private bool insertarCliente(vClienteEntidad cliente)
        {
            Persona personaBase;
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                var resultado = from p in dtc.Persona
                                where p.Cedula == cliente.Cedula
                                select p;
                personaBase = (Persona)resultado.FirstOrDefault();
            }

            if (personaBase == null)
            {
                personaBase = new Persona();
                personaBase.Cedula = cliente.Cedula;
                personaBase.Nombre = cliente.Nombre;
                personaBase.Apellido = cliente.Apellido;
                using (FacturasDataContext dtc = new FacturasDataContext())
                {
                    dtc.Connection.Open();
                    dtc.Transaction = dtc.Connection.BeginTransaction();
                    using (DbTransaction tscope = dtc.Transaction)
                    {
                        try
                        {
                            dtc.Persona.InsertOnSubmit(personaBase);
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
                }
            }
            else
            {
                personaBase.Nombre = cliente.Nombre;
                personaBase.Apellido = cliente.Apellido;
            }

            Cliente clienteBase = new Cliente();
            clienteBase.Cedula = cliente.Cedula;
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

        private bool actualizarCliente(vClienteEntidad cliente)
        {
            Persona personaBase = new Persona();

            Cliente clienteBase = new Cliente();


            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        var resultado1 = from p in dtc.Persona
                                         where p.Cedula == cliente.Cedula
                                         select p;

                        personaBase = (Persona)resultado1.First();

                        personaBase.Nombre = cliente.Nombre;
                        personaBase.Apellido = cliente.Apellido;

                        var resultado2 = from p in dtc.Cliente
                                         where p.Cedula == cliente.Cedula
                                         select p;

                        clienteBase = (Cliente)resultado2.First();


                        clienteBase.Telefono = cliente.Telefono;
                        clienteBase.Postal = cliente.Postal;
                        clienteBase.Direccion = cliente.Direccion;
                        clienteBase.Correo = cliente.Correo; ;

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

        private bool borrarCliente(vClienteEntidad cliente)
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
