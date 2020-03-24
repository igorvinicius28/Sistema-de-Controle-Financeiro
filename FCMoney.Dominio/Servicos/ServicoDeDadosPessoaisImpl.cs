using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades;
using FCMoney.Dominio.Interfaces.Servicos;
using FCMoney.Dominio.Servicos;

namespace FCMoney.Repositorio.Servicos.Servicos
{
    public class ServicoDeDadosPessoaisImpl :ServicoBase<DadosPessoais>,  IServicoDeDadosPessoais
    {
        private IRepositorioDadosPessoais _repositorioDadosPessoais;
        
        public ServicoDeDadosPessoaisImpl(IRepositorioDadosPessoais repositorioDadosPessoais)
            :base(repositorioDadosPessoais)
        {
            _repositorioDadosPessoais = repositorioDadosPessoais;
        }

       public DadosPessoais ConsultePorId(string id)
        {
            return _repositorioDadosPessoais.ConsultePorId(id);
        }
    }
}
