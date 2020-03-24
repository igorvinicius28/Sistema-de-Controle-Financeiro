using FCMoney.Dominio.Enumeradores;
using System;
using System.ComponentModel.DataAnnotations;

namespace PrototipoLogin.Models.Contas
{
    public class DtoContasAPagarReceber
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public double? Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataDoPagamento{ get; set; }

        public EnumTipo Tipo { get; set; }

        [Display(Name = "Foi Paga")]
        public bool FoiPagaOuRecebida { get; set; }
    }
}