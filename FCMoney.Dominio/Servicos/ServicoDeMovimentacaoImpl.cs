using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Interfaces.Servicos;
using FCMoney.Dominio.Servicos;

namespace FCMoney.Repositorio.Servicos.Servicos
{
    public class ServicoDeMovimentacaoImpl : ServicoBase<Movimentacao>, IServicoDeMovimentacao
    {
        private IMovimentacoesRepositorio _repositorioMovimentacoes;

        public ServicoDeMovimentacaoImpl(IMovimentacoesRepositorio repositorioMovimentacao)
              : base(repositorioMovimentacao)
        {
            _repositorioMovimentacoes = repositorioMovimentacao;
        }



    }
}
