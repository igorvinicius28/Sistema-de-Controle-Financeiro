using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Dominio.Entidades.Movimentacoes
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public DateTime DataCadastro { get; set; }

        public double? Valor { get; set; }

        public string Descricao { get; set; }

        public EnumTipo Tipo { get; set; }

        public string UsuarioId { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
