using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrototipoLogin.Models.Usuario
{
    public class DtoRecuperarSenha
    {
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string Password { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}