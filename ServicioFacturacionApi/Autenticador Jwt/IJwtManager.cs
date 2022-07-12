using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Autenticador_Jwt
{
    public interface IJwtManager
    {
        string Autenticador(string usuario, string contraseña);
    }
}