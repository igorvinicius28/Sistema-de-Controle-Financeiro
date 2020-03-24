using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Enumeradores;
using FCMoney.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FCMoney.Dominio.Servicos
{
    public class ServicoBase<TEntity> : IDisposable, IServicoBase<TEntity> where TEntity : class
    {
        private readonly IBaseRepositorio<TEntity> _repositorio;

        public ServicoBase(IBaseRepositorio<TEntity> repositorio)
        {
            _repositorio = repositorio;
        }

        public void Atualizar(TEntity obj)
        {
            _repositorio.Atualizar(obj);
        }

        public void Cadastrar(TEntity obj)
        {
            _repositorio.Cadastrar(obj);
        }

        public List<TEntity> ConsulteLista()
        {
            return _repositorio.ConsulteLista().ToList();
        }

        public TEntity ConsultePorId(int id)
        {
            return _repositorio.ConsultarPorId(id);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
        }

        public void Excluir(TEntity obj)
        {
            _repositorio.Excluir(obj);
        }

        /// <summary>
        /// Metodo responsavel por obter os valores calculados por mes.
        /// </summary>
        /// <param name="movimentacoes">Lista de movimentacoes.</param>
        /// <param name="contas">Lista de contas.</param>
        /// <param name="mes">Mes a ser buscado.</param>
        /// <returns>Retorna o valor calculado daquele mes.</returns>
        public double? ObtenhaValoresTotalPorMes(List<Movimentacao> movimentacoes, List<Contas> contas, int mes)
        {
            var valorReceita = ConsulteValoresReceitaMes(movimentacoes, contas, mes);
            var valorDespesa = ConsulteValoresDespesasMes(movimentacoes, contas, mes);

            return  valorReceita - valorDespesa;
        }

        private double? ConsulteValoresReceitaMes(List<Movimentacao> movimentacoes, List<Contas> contas, int mes)
        {
            var ano = DateTime.Now.Year;

            var valoresMovimentacao = movimentacoes
                .Where(x => x.Tipo == EnumTipo.ENTRADA &&
                            x.DataCadastro.Year == ano && 
                            x.DataCadastro.Month == mes)
                .Sum(x => x.Valor) ?? 0;

            var valoresContasRecebidas = contas
                .Where(x => x.Tipo == EnumTipo.AHRECEBER &&
                            x.FoiPagaOuRecebida == true &&
                            x.DataCadastro.Year == ano &&
                            x.DataCadastro.Month == mes)
                .Sum(x => x.Valor) ?? 0;

            return valoresMovimentacao + valoresContasRecebidas;
        }

        private double? ConsulteValoresDespesasMes(List<Movimentacao> movimentacoes, List<Contas> contas, int mes)
        {
            var ano = DateTime.Now.Year;

            var valoresMovimentacao = movimentacoes
                .Where(x => x.Tipo == EnumTipo.SAIDA && 
                            x.DataCadastro.Year == ano &&
                            x.DataCadastro.Month == mes)
                .Sum(x => x.Valor);

            var valoresContasPagas = contas
                .Where(x => x.Tipo == EnumTipo.AHPAGAR &&
                            x.FoiPagaOuRecebida == true && 
                            x.DataCadastro.Year == ano && 
                            x.DataCadastro.Month == mes)
                .Sum(x => x.Valor);

            return valoresMovimentacao + valoresContasPagas;
        }
    }
}
