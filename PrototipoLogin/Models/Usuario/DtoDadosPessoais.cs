using FCMoney.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrototipoLogin.Models.Usuario
{
    public class DtoDadosPessoais
    {
        public string Nome { get; set; }

       // public Endereco Endereco { get; set; }

        public string CEP { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }
    }
}