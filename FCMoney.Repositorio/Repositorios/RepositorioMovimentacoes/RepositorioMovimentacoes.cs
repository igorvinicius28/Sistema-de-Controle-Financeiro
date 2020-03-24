using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Repositorio.Contexto;
using FCMoney.Repositorio.Repositorios.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes
{
    public class RepositorioMovimentacoes : BaseRepositorio<Movimentacao>, IMovimentacoesRepositorio
    {
        private ApplicationDbContext Contexto;

        public RepositorioMovimentacoes(ApplicationDbContext fcMoney) : base(fcMoney)
        {

        }

        public RepositorioMovimentacoes()
        {
            Contexto = new ApplicationDbContext();
        }
    }
}
