using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Enumeradores;
using System;


namespace FCMoney.Dominio.Entidades.Movimentacoes
{
    public class Contas
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public double? Valor { get; set; }

        public bool FoiPagaOuRecebida { get; set; }

        public DateTime DataDoPagamento { get; set; }

        public DateTime DataCadastro { get; set; }

        public string UsuarioId { get; set; }

        public EnumTipo Tipo { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
    }
}
