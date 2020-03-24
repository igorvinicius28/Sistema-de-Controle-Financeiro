using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Dominio.Entidades
{
    public class Endereco
    {
        public int Id { get; set; }

        public string CEP { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public int IdCidade { get; set; }

        public Cidade Cidade { get; set; }

    }

}
