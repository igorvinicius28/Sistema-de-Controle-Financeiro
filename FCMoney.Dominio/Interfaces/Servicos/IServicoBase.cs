using FCMoney.Dominio.Entidades.Movimentacoes;
using System.Collections.Generic;

namespace FCMoney.Dominio.Interfaces.Repositorios
{
    public interface IServicoBase<TEntity> where TEntity : class
    {
        void Cadastrar(TEntity obj);
        TEntity ConsultePorId(int id);
        List<TEntity> ConsulteLista();
        void Atualizar(TEntity obj);
        void Excluir(TEntity obj);
        double? ObtenhaValoresTotalPorMes(List<Movimentacao> movimentacoes, List<Contas> contas, int mes);
        void Dispose();
    }
}
