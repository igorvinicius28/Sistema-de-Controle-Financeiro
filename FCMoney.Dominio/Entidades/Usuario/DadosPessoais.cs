using FCMoney.Dominio.Entidades.Autenticacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Dominio.Entidades
{
    public class DadosPessoais : Entidade
    {
        public int Id { get; set; }

      //  public DateTime DataNascimento { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public int IdEndereco { get; set; }

//     public Endereco Endereco { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }

        public string UsuarioId { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
