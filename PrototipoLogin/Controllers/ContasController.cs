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

        /***************************** CONTAS A PAGAR ***************************************/

        // GET: Contas
        public ActionResult ContasAPagar()
        {
            ServicoDeContasImpl servico = new ServicoDeContasImpl(new RepositorioContas());
            var lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.AHPAGAR).ToList();
            var listaDtoEntrada = new List<DtoContasAPagarReceber>();
            lista.ForEach(x =>
            {
                var dtoEntrada = new DtoContasAPagarReceber()
                {
                    Id = x.Id,
                    DataDoPagamento = x.DataDoPagamento,
                    Descricao = x.Descricao,
                    Tipo = EnumTipo.ENTRADA,
                    FoiPagaOuRecebida = x.FoiPagaOuRecebida,
                    Valor = x.Valor
                };
                listaDtoEntrada.Add(dtoEntrada);
            });

            return View(listaDtoEntrada);
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
                    DataCadastro = dtoContasAPagarReceber.DataCadastro,
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

        public ActionResult Edit(int id)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            var movimentacaoAhPagarReceber = servico.ConsultePorId(id);

            var dtoContasAPagarReceber = new DtoContasAPagarReceber
            {
                Id = movimentacaoAhPagarReceber.Id,
                Descricao = movimentacaoAhPagarReceber.Descricao,
                DataCadastro = movimentacaoAhPagarReceber.DataCadastro,
                FoiPagaOuRecebida = movimentacaoAhPagarReceber.FoiPagaOuRecebida,
                Valor = movimentacaoAhPagarReceber.Valor,
                Tipo = movimentacaoAhPagarReceber.Tipo
            };

            if(movimentacaoAhPagarReceber.Tipo == EnumTipo.AHPAGAR)
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
                    DataDoPagamento = dtoContasAPagarReceber.DataDoPagamento,
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

        public ActionResult excluirs(int id)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            var movimentacaoAhPagarReceber = servico.ConsultePorId(id);

            var dtoEntrada = new DtoContasAPagarReceber
            {
                Id = movimentacaoAhPagarReceber.Id,
                Descricao = movimentacaoAhPagarReceber.Descricao,
                DataCadastro = movimentacaoAhPagarReceber.DataCadastro,
                Valor = movimentacaoAhPagarReceber.Valor,
                Tipo = movimentacaoAhPagarReceber.Tipo
            };

            return PartialView("~/Views/Contas/ConfirmacaoExcluirContaModal.cshtml", dtoEntrada);
        }

        public ActionResult excluir(int id)
        {
            var servico = new ServicoDeContasImpl(new RepositorioContas());

            var movimentacaoAhPagarReceber = servico.ConsultePorId(id);

            servico.Excluir(movimentacaoAhPagarReceber);

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        /***************************** CONTAS A Receber  **********************************/

        // GET: Contas
        public ActionResult ContasAReceber()
        {
            ServicoDeContasImpl servico = new ServicoDeContasImpl(new RepositorioContas());
            var lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.AHRECEBER).ToList();
            var listaDtoAhReceber = new List<DtoContasAPagarReceber>();
            lista.ForEach(x =>
            {
                var dtoAhReceber = new DtoContasAPagarReceber()
                {
                    Id = x.Id,
                    DataDoPagamento = x.DataDoPagamento,
                    Descricao = x.Descricao,
                    Tipo = EnumTipo.ENTRADA,
                    FoiPagaOuRecebida = x.FoiPagaOuRecebida,
                    Valor = x.Valor
                };
                listaDtoAhReceber.Add(dtoAhReceber);
            });

            return View(listaDtoAhReceber);
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
                    DataCadastro = dtoContasAPagarReceber.DataCadastro,
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
    }
}