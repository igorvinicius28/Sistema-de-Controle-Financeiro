using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Interfaces.Servicos;
using FCMoney.Dominio.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMoney.Repositorio.Servicos.Servicos
{
    public class ServicoDeContasImpl : ServicoBase<Contas>, IServicoDeContas
    {
        private IContasRepositorio _repositorioContas;

        public ServicoDeContasImpl(IContasRepositorio repositorioDeConta)
            :base(repositorioDeConta)
        {
            _repositorioContas = repositorioDeConta;
        }
    }
}
