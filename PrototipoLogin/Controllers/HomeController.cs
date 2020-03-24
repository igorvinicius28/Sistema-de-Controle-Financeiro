using FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes;
using FCMoney.Repositorio.Servicos.Servicos;
using PrototipoLogin.Models.Index;
using System.Collections.Generic;
using System.Web.Mvc;
using FCMoney.Dominio.Entidades.Movimentacoes;
using System.Linq;
using FCMoney.Dominio.Enumeradores;

namespace PrototipoLogin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static ServicoDeMovimentacaoImpl _servicoMovimentacao;
        private static ServicoDeContasImpl _servicoContas;

        public HomeController()
        {
            CrieRepositorios();
        }

        [Authorize]
        public ActionResult Index()
        {

            var listaMovimentacoes = _servicoMovimentacao.ConsulteLista();
            var listaContas = _servicoContas.ConsulteLista();

            var valorEntrada = ObtenhaMovimentacoesEContasEntrada(listaMovimentacoes, listaContas);
            var valorDespesa = ObtenhaMovimentacoesEContasSaida(listaMovimentacoes, listaContas);
            var valorTotal = ObtenhaSaldoAtual(listaMovimentacoes, listaContas);
            
            var valoresJaneiro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 1);
            var valoresFevereiro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 2);
            var valoresMarco = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 3);
            var valoresAbril = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 4);
            var valoresMaio = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 5);
            var valoresjunho = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 6);
            var valoresJulho = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 7);
            var valoresAgosto = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 8);
            var valoresSetembro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 9);
            var valoresOutubro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 10);
            var valoresNovembro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 11);
            var valoresDezembro = _servicoMovimentacao.ObtenhaValoresTotalPorMes(listaMovimentacoes, listaContas, 12);


            var dtoMenuPrincipal = new DtoMenuPrincipal
            {
                ValorMovimentacaoEntrada = valorEntrada != null ? valorEntrada.ToString() : "0",
                ValorMovimentacaoSaida = valorDespesa != null ? valorDespesa.ToString() : "0",
                SaldoAtual = valorTotal != null ? valorTotal.ToString() : "0",                
                valoresJaneiro = valoresJaneiro != null ? valoresJaneiro.ToString() : "0",
                valoresFevereiro = valoresFevereiro != null ? valoresFevereiro.ToString() : "0",
                valoresMarco = valoresMarco != null ? valoresMarco.ToString() : "0",
                valoresAbril = valoresAbril != null ? valoresAbril.ToString() : "0",
                valoresMaio = valoresMaio != null ? valoresMaio.ToString() : "0",
                valoresjunho = valoresjunho != null ? valoresjunho.ToString() : "0",
                valoresJulho = valoresJulho != null ? valoresJulho.ToString() : "0",
                valoresAgosto = valoresAgosto != null ? valoresAgosto.ToString() : "0",
                valoresSetembro = valoresSetembro != null ? valoresSetembro.ToString() : "0",
                valoresOutubro = valoresOutubro != null ? valoresOutubro.ToString() : "0",
                valoresNovembro = valoresNovembro != null ? valoresNovembro.ToString() : "0",
                valoresDezembro = valoresDezembro != null ? valoresDezembro.ToString() : "0",
            };
            return View(dtoMenuPrincipal);
        }

        public ActionResult SignOut()
        {
            ViewBag.Message = "Sair";

            return View();
        }

        // Conversor de entradas

        private double? ObtenhaValoresEntrada(List<Movimentacao> movimentacoes)
        {
            return movimentacoes.Where(x => x.Tipo == EnumTipo.ENTRADA).Sum(x => x.Valor) ?? 0;
        }

        private double? ObtenhaValoresEntradaPago(List<Contas> contas)
        {
            return contas.Where(x => x.Tipo == EnumTipo.AHRECEBER && x.FoiPagaOuRecebida == true).Sum(x => x.Valor) ?? 0;
        }

        private double? ObtenhaMovimentacoesEContasEntrada(List<Movimentacao> movimentacoes, List<Contas> contas)
        {
            return ObtenhaValoresEntrada(movimentacoes) + ObtenhaValoresEntradaPago(contas);
        }

        // Conversor de Saida

        public double? ObtenhaValoresSaida(List<Movimentacao> movimentacoes)
        {
            return movimentacoes.Where(x => x.Tipo == EnumTipo.SAIDA).Sum(x => x.Valor) ?? 0;
        }

        public double? ObtenhaValoresSaidaPago(List<Contas> contas)
        {
            return contas.Where(x => x.Tipo == EnumTipo.AHPAGAR && x.FoiPagaOuRecebida == true).Sum(x => x.Valor) ?? 0;
        }

        public double? ObtenhaMovimentacoesEContasSaida(List<Movimentacao> movimentacoes, List<Contas> contas)
        {        
            return ObtenhaValoresSaida(movimentacoes) + ObtenhaValoresSaidaPago(contas);
        }

        public double? ObtenhaSaldoAtual(List<Movimentacao> movimentacoes, List<Contas> contas)
        {
            var valoresEntrada = ObtenhaMovimentacoesEContasEntrada(movimentacoes, contas);
            var valoresSaida = ObtenhaMovimentacoesEContasSaida(movimentacoes, contas);
            return valoresEntrada - valoresSaida;
        }

        private void CrieRepositorios()
        {
            _servicoMovimentacao = _servicoMovimentacao ?? new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            _servicoContas = _servicoContas ?? new ServicoDeContasImpl(new RepositorioContas());
        }
    }
}