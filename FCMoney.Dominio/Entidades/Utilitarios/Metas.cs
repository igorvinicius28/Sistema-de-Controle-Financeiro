using System;

namespace FCMoney.Dominio.Entidades.Utilitarios
{
    public class Metas
    {
        public int Id { get; set; }

        public Decimal ValorRecceita { get; set; }

        public string Descricao { get; set; }

        public decimal ValorDespesas { get; set; }

    //    public Usuario.Usuario Usuario { get; set; }

        public DateTime MesDaMeta { get; set; }

        public int IDUsuario { get; set; }
    }
}
