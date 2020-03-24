using FCMoney.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Dominio.Contratos
{
    public interface IRepositorioDadosPessoais :  IBaseRepositorio<DadosPessoais>
    {
        DadosPessoais ConsultePorId(string id);
    }
}
