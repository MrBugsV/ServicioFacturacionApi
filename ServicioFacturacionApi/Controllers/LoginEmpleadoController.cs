using ServicioFacturacionApi.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ServicioFacturacionApi.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginEmpleadoController : ApiController
    {
        #region Servicios REST
        //// GET: api/LoginEmpleado
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/LoginEmpleado/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/LoginEmpleado
        public IHttpActionResult Post([FromBody] LoginEntidad login)
        {
            return insertarLoginEmpleados(login);
        }

        [HttpPost]
        [Route("api/LoginEmpleado/Login")]
        [AllowAnonymous]
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

        // PUT: api/LoginEmpleado
        public IHttpActionResult Put(LoginEntidad login)
        {
            return actualizarLoginEmpleado(login);
        }


        //// DELETE: api/LoginEmpleado/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        #region Metodos Base de Datos

        private IHttpActionResult insertarLoginEmpleados(LoginEntidad loginEntidad)
        {
            LoginEmpleado loginBase = new LoginEmpleado();
            loginBase.Usuario = loginEntidad.Usuario;
            loginBase.Contraseña = loginEntidad.Contraseña;

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
        
        private bool verificarLoginEmpleado(LoginEntidad login)
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

        private IHttpActionResult actualizarLoginEmpleado(LoginEntidad login)
        {
            LoginEmpleado loginBase;
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                dtc.Connection.Open();
                dtc.Transaction = dtc.Connection.BeginTransaction();
                using (DbTransaction tscope = dtc.Transaction)
                {
                    try
                    {
                        var resultado = from p in dtc.LoginEmpleado
                                        where p.Usuario == login.Usuario
                                        select p;

                        loginBase = (LoginEmpleado)resultado.First();

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
