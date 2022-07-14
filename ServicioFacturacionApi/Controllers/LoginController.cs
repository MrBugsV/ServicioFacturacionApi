using ServicioFacturacionApi.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ServicioFacturacionApi.Controllers
{
    [AllowAnonymous]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {

        #region Servicios REST

        [HttpPost]
        [Route("api/LoginEmpleado")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult loginEmpleado(LoginEntidad login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (verificarLoginEmpleado(login))
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario);
                return Ok(token);
            }
            else
                return Unauthorized();
        }

        [HttpPost]
        [Route("api/LoginCliente")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult loginCliente(LoginEntidad login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginClienteEntidad cliente;
            if (verificarLoginCliente(login,out cliente))
            {
                cliente.Token = TokenGenerator.GenerateTokenJwt(login.Usuario);
                return Ok(cliente);
            }
            else
                return Unauthorized(null);
        }

        [HttpPost]
        [Route("api/RegistrarEmpleado")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult registrarEmpleado(LoginEntidad login)
        {
            return registrarLoginEmpleado(login);
        }

        [HttpPost]
        [Route("api/RegistrarCliente")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult registrarCliente(LoginEntidad login)
        {
            return registrarLoginCliente(login);
        }

        #endregion

        #region Métodos Base de Datos

        public bool verificarLoginEmpleado(LoginEntidad login)
        {
            LoginEmpleado empleado;
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                var resultado = from p in dtc.LoginEmpleado
                                where p.Usuario == login.Usuario
                                    && p.Contraseña == login.Contraseña
                                select p;
                empleado = (LoginEmpleado)resultado.FirstOrDefault();
            }
            return empleado != null;
        }

        public bool verificarLoginCliente(LoginEntidad login, out LoginClienteEntidad loginCliente)
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

        public IHttpActionResult registrarLoginEmpleado(LoginEntidad login)
        {
            LoginEmpleado loginBase = new LoginEmpleado();
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
                        dtc.LoginEmpleado.InsertOnSubmit(loginBase);
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

        public IHttpActionResult registrarLoginCliente(LoginEntidad login)
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

        #endregion
    }
}
