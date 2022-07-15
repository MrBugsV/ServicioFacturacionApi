using ServicioFacturacionApi.Entidades;
using System;
using System.Data.Common;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ServicioFacturacionApi.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginClienteController : ApiController
    {
        #region Servicios REST
        // GET: api/LoginCliente
        //public string Get()
        //{
        //    return "";
        //}

        //// GET: api/LoginCliente/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/LoginCliente
        public IHttpActionResult Post([FromBody] LoginEntidad login)
        {
            return insertarLoginCliente(login);
        }

        [HttpPost]
        [Route("api/LoginCliente/Login")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult loginCliente(LoginEntidad login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginClienteEntidad cliente;
            if (verificarLoginCliente(login, out cliente))
            {
                cliente.Token = TokenGenerator.GenerateTokenJwt(login.Usuario);
                return Ok(cliente);
            }
            else
                return Unauthorized(null);
        }

        // PUT: api/LoginCliente
        public IHttpActionResult Put(LoginEntidad login)
        {
            return actualizarLoginCliente(login);
        }

        //// DELETE: api/LoginCliente/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        #region Métodos Base de Datos

        private bool verificarLoginCliente(LoginEntidad login, out LoginClienteEntidad loginCliente)
        {
            LoginCliente loginClienteBase;
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                var resultado = from p in dtc.LoginCliente
                                where p.Usuario == login.Usuario
                                    && p.Contraseña == login.Contraseña
                                select p;
                loginClienteBase = (LoginCliente)resultado.FirstOrDefault();

                if (loginClienteBase != null)
                {
                    var cliente = dtc.Cliente.Where(p => p.Cedula == loginClienteBase.Usuario).FirstOrDefault();
                    loginCliente = new LoginClienteEntidad(
                        cliente.Nombre,
                        cliente.Apellido,
                        cliente.Correo,
                        ""
                    );
                    return true;
                }
            }
            loginCliente = null;
            return false;
        }

        private IHttpActionResult insertarLoginCliente(LoginEntidad login)
        {
            LoginCliente loginBase = new LoginCliente();
            loginBase.Usuario = login.Usuario;
            loginBase.Contraseña = login.Contraseña;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        dtc.LoginCliente.InsertOnSubmit(loginBase);
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
                return Ok(true);
            }
        }

        private IHttpActionResult actualizarLoginCliente(LoginEntidad login)
        {
            LoginCliente loginBase;

            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        var resultado = from p in dtc.LoginCliente
                                        where p.Usuario == login.Usuario
                                        select p;

                        loginBase = (LoginCliente)resultado.First();

                        if (loginBase.Contraseña != login.Contraseña)
                            loginBase.Contraseña = login.Contraseña;


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
                return Ok(true);
            }
        }

        #endregion

    }
}
