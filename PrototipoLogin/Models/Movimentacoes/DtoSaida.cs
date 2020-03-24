using FCMoney.Dominio.Enumeradores;
using System;

namespace PrototipoLogin.Models.Movimentacoes
{
    public class DtoSaida
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public double? Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public EnumTipo Tipo { get; set; }
    }
}