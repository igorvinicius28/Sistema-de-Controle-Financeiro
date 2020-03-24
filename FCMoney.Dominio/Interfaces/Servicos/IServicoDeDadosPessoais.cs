using FCMoney.Dominio.Entidades;
using FCMoney.Dominio.Interfaces.Repositorios;

namespace FCMoney.Dominio.Interfaces.Servicos
{
    public interface IServicoDeDadosPessoais : IServicoBase<DadosPessoais>
    {
        DadosPessoais ConsultePorId(string id);
    }
}
