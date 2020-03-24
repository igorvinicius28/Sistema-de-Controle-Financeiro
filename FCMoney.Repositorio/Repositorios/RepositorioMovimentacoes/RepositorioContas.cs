using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Repositorio.Contexto;
using FCMoney.Repositorio.Repositorios.Util;
using System;


namespace FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes
{
    public class RepositorioContas : BaseRepositorio<Contas>, IContasRepositorio
    {
        private ApplicationDbContext Contexto;

        public RepositorioContas(ApplicationDbContext fcMoney) : base(fcMoney)
        {

        }


        public RepositorioContas()
        {
            Contexto = new ApplicationDbContext();
        }

        public void Cadastrar(Contas contas)
        {
            Contexto.Set<Contas>().Add(contas);
            Contexto.SaveChanges();

        }
    }
}
