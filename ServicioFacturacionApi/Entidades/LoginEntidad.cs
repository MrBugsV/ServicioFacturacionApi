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
}