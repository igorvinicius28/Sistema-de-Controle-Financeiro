using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Enumeradores;
using FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes;
using FCMoney.Repositorio.Servicos.Servicos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog.Fluent;
using PrototipoLogin.Identity;
using PrototipoLogin.Models.Contas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrototipoLogin.Controllers
{
    [Authorize]
    public class ContasController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MovimentacoesController));

        public ContasController()
        {
        }

        public ContasController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /***************************** CONTAS A RECEBER ***************************************/

        // GET: Movimentacoes
        public ActionResult IndexContaEntrada()
        {
            return View();
        }

        public ActionResult ObtenhaListaContasAhReceber()
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());
            var lista = new List<Contas>();

            lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.AHRECEBER).ToList();

            var listaDtoContasAPagarReceber = new List<DtoContasAPagarReceber>();
            lista.ForEach(x =>
            {
                var dtoConta = new DtoContasAPagarReceber()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro.ToString(),
                    DataDoPagamento = x.DataDoPagamento.ToShortDateString(),
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    FoiPagaOuRecebida = x.FoiPagaOuRecebida,
                    Valor = x.Valor
                };
                listaDtoContasAPagarReceber.Add(dtoConta);
            });

            return Json(new { data = listaDtoContasAPagarReceber }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAhReceber()
        {
            var dtoContasAPagarReceber = new DtoContasAPagarReceber();
            return PartialView("~/Views/Contas/FormularioAhReceber.cshtml", dtoContasAPagarReceber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAhReceber(DtoContasAPagarReceber dtoContasAPagarReceber)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            try
            {
                //conversor
                var movimentacao = new Contas
                {
                    Descricao = dtoContasAPagarReceber.Descricao,
                    DataCadastro = DateTime.Now,
                    DataDoPagamento = DateTime.Parse(dtoContasAPagarReceber.DataDoPagamento),
                    Valor = dtoContasAPagarReceber.Valor,
                    Tipo = dtoContasAPagarReceber.Tipo,
                    FoiPagaOuRecebida = dtoContasAPagarReceber.FoiPagaOuRecebida,
                };

                servico.Cadastrar(movimentacao);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmacaoExcluirModal(int id)
        {
            var dtoContasAPagarReceber = new DtoContasAPagarReceber
            {
                Descricao = id.ToString(),
            };
            return PartialView("~/Views/Contas/ConfirmacaoExcluirContaModal.cshtml", dtoContasAPagarReceber);
        }

        /***************************** CONTAS A PAGAR ***************************************/

        // GET: Movimentacoes
        public ActionResult IndexContaSaida()
        {
            return View();
        }

        public ActionResult ObtenhaListaContasAhPagar()
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());
            var lista = new List<Contas>();

            lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.AHPAGAR).ToList();

            var listaDtoContasAPagarReceber = new List<DtoContasAPagarReceber>();
            lista.ForEach(x =>
            {
                var dtoConta = new DtoContasAPagarReceber()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro.ToShortDateString(),
                    DataDoPagamento = x.DataDoPagamento.ToShortDateString(),
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    FoiPagaOuRecebida = x.FoiPagaOuRecebida,
                    Valor = x.Valor
                };
                listaDtoContasAPagarReceber.Add(dtoConta);
            });

            return Json(new { data = listaDtoContasAPagarReceber }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAhPagar()
        {
            var DtoEntrada = new DtoContasAPagarReceber();
            return PartialView("~/Views/Contas/FormularioAhPagar.cshtml", DtoEntrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAhPagar(DtoContasAPagarReceber dtoContasAPagarReceber)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            try
            {
                //conversor
                var movimentacao = new Contas
                {
                    Descricao = dtoContasAPagarReceber.Descricao,
                    DataCadastro = DateTime.Now,
                    DataDoPagamento = DateTime.Parse(dtoContasAPagarReceber.DataDoPagamento),
                    Valor = dtoContasAPagarReceber.Valor,
                    Tipo = dtoContasAPagarReceber.Tipo,
                    FoiPagaOuRecebida = dtoContasAPagarReceber.FoiPagaOuRecebida
                };

                servico.Cadastrar(movimentacao);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        /**************************** METODOS GENERICOS ************************************/

        public ActionResult Edit(int id)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            var movimentacaoAhPagarReceber = servico.ConsultePorId(id);

            var dtoContasAPagarReceber = new DtoContasAPagarReceber
            {
                Id = movimentacaoAhPagarReceber.Id,
                Descricao = movimentacaoAhPagarReceber.Descricao,
                DataCadastro = movimentacaoAhPagarReceber.DataCadastro.ToShortDateString(),
                DataDoPagamento = movimentacaoAhPagarReceber.DataDoPagamento.ToShortDateString(),
                FoiPagaOuRecebida = movimentacaoAhPagarReceber.FoiPagaOuRecebida,
                Valor = movimentacaoAhPagarReceber.Valor,
                Tipo = movimentacaoAhPagarReceber.Tipo
            };

            if (movimentacaoAhPagarReceber.Tipo == EnumTipo.AHPAGAR)
            {
                return PartialView("~/Views/Contas/FormularioAhPagar.cshtml", dtoContasAPagarReceber);
            }
            return PartialView("~/Views/Contas/FormularioAhReceber.cshtml", dtoContasAPagarReceber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DtoContasAPagarReceber dtoContasAPagarReceber)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());
            try
            {
                var movimentacaoAhPagarReceber = new Contas
                {
                    Id = dtoContasAPagarReceber.Id,
                    Descricao = dtoContasAPagarReceber.Descricao,
                    DataCadastro = DateTime.Parse(dtoContasAPagarReceber.DataCadastro),
                    DataDoPagamento = DateTime.Parse(dtoContasAPagarReceber.DataDoPagamento),
                    FoiPagaOuRecebida = dtoContasAPagarReceber.FoiPagaOuRecebida,
                    Tipo = dtoContasAPagarReceber.Tipo,
                    Valor = dtoContasAPagarReceber.Valor
                };

                servico.Atualizar(movimentacaoAhPagarReceber);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            var contas = servico.ConsultePorId(id);

            servico.Excluir(contas);
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}