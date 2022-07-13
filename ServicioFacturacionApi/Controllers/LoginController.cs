using ServicioFacturacionApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServicioFacturacionApi.Controllers
{
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        [HttpPost]
        public IHttpActionResult login(LoginEntidad login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (verificarLogin(login))
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario);
            }

            return Ok();
        }

        public bool verificarLogin(LoginEntidad login)
        {
            return true;
        }
    }
}
