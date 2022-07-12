using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ServicioFacturacionApi.Autenticador_Jwt;
using ServicioFacturacionApi.Entidades;

namespace ServicioFacturacionApi.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("api/Usuario")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Autenticar([FromBody] vUsuarioEntidad usuario)
        {
            return autenticarUsuario(usuario);
        }

        private IHttpActionResult autenticarUsuario(vUsuarioEntidad usuario)
        {
            vUsuarios usuarioBase;
            using (FacturasDataContext dtc = new FacturasDataContext())
            {
                var resultado = from p in dtc.vUsuarios
                                where p.Cedula == usuario.Cedula
                                    && p.Contraseña == usuario.Contraseña
                                select p;
                usuarioBase = (vUsuarios)resultado.FirstOrDefault();
            }

            //if (usuarioBase != null)
            //{
            //    var token = this.jwtManager.Autenticador(usuarioBase.Cedula, usuarioBase.Contraseña);
            //    if (token != null)
            //    {
            //        return Ok(token);
            //    }
            //}
            return Unauthorized();
        }

        // GET: api/Usuario
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Usuario/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Usuario
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Usuario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Usuario/5
        public void Delete(int id)
        {
        }
    }
}
