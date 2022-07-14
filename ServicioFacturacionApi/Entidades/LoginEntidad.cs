using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServicioFacturacionApi.Entidades
{
    public class LoginEntidad
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contraseña { get; set; }

        public LoginEntidad()
        {

        }

        public LoginEntidad(string usuario, string contraseña)
        {
            Usuario = usuario;
            Contraseña = contraseña;
        }
    }

    public class LoginClienteEntidad
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Token { get; set; }
        public LoginClienteEntidad()
        {

        }

        public LoginClienteEntidad(string nombre, string apellido, string correo, string token)
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Token = token;
        }
    }
}