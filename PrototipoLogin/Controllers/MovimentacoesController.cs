using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Enumeradores;
using FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes;
using FCMoney.Repositorio.Servicos.Servicos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrototipoLogin.Identity;
using PrototipoLogin.Models.Movimentacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Log = NLog.Fluent.Log;

namespace PrototipoLogin.Controllers
{
    [Authorize]
    public class MovimentacoesController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MovimentacoesController));

        public MovimentacoesController()
        {
        }

        public MovimentacoesController(ApplicationUserManager userManager)
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

        /************************* IMIGRACAO ****************************/

        // GET: Movimentacoes
        public ActionResult IndexEntrada()
        {
            return View();
        }

        public ActionResult ObtenhaListaEntrada(string id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            var lista = new List<Movimentacao>();

            lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.ENTRADA).ToList();

            var listaDtoEntrada = new List<DtoEntrada>();
            lista.ForEach(x =>
            {
                var dtoEntrada = new DtoEntrada()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro.ToShortDateString(),
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    Valor = x.Valor
                };
                listaDtoEntrada.Add(dtoEntrada);
            });

            return Json(new { data = listaDtoEntrada }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var DtoEntrada = new DtoEntrada();
            return PartialView("~/Views/Movimentacoes/FormularioEntrada.cshtml", DtoEntrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEntrada(DtoEntrada dtoEntrada)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            try
            {
                //conversor
                var movimentacao = new Movimentacao
                {
                    Descricao = dtoEntrada.Descricao,
                    DataCadastro = DateTime.Parse(dtoEntrada.DataCadastro),
                    Valor = dtoEntrada.Valor,
                    Tipo = EnumTipo.ENTRADA

                };

                servico.Cadastrar(movimentacao);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacao = servico.ConsultePorId(id);

            servico.Excluir(movimentacao);
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacao = servico.ConsultePorId(id);

            if(movimentacao.Tipo == EnumTipo.ENTRADA) { 

                var dtoEntrada = new DtoEntrada
                {
                    Id = movimentacao.Id,
                    Descricao = movimentacao.Descricao,
                    DataCadastro = movimentacao.DataCadastro.ToShortDateString(),
                    Valor = movimentacao.Valor,
                    Tipo = movimentacao.Tipo
                };

                return PartialView("~/Views/Movimentacoes/FormularioEntrada.cshtml", dtoEntrada);
            }
            else
            {
                var dtoSaida = new DtoSaida
                {
                    Id = movimentacao.Id,
                    Descricao = movimentacao.Descricao,
                    DataCadastro = movimentacao.DataCadastro.ToShortDateString(),
                    Valor = movimentacao.Valor,
                    Tipo = movimentacao.Tipo
                };

                return PartialView("~/Views/Movimentacoes/FormularioSaida.cshtml", dtoSaida);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DtoEntrada dtoEntrada)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            try
            {
                var movimentacaoEntrada = new Movimentacao
                {
                    Id = dtoEntrada.Id,
                    Descricao = dtoEntrada.Descricao,
                    DataCadastro = DateTime.Parse(dtoEntrada.DataCadastro),
                    Valor = dtoEntrada.Valor,
                    Tipo = dtoEntrada.Tipo
                };

                servico.Atualizar(movimentacaoEntrada);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmacaoExcluirModal(int id)
        {
            var dtoEntrada = new DtoEntrada
            {
                Descricao = id.ToString(),
            };
            return PartialView("~/Views/Movimentacoes/ConfirmacaoExcluirModal.cshtml", dtoEntrada);
        }

        /// <summary>

        /************************* MOVIMENTACOES DE ENTRADA ****************************/


        public ActionResult excluir(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacaoEntrada = servico.ConsultePorId(id);

            servico.Excluir(movimentacaoEntrada);

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        /********************************************** MOVIMENTACOES DE SAIDA ***************************************/

        // GET: Movimentacoes
        public ActionResult IndexSaida()
        {
            return View();
        }

        public ActionResult ObtenhaListaSaida()
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            var lista = new List<Movimentacao>();

            lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.SAIDA).ToList();

            var listaDtoSaida = new List<DtoSaida>();
            lista.ForEach(x =>
            {
                var dtoSaida = new DtoSaida()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro.ToShortDateString(),
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    Valor = x.Valor
                };
                listaDtoSaida.Add(dtoSaida);
            });

            return Json(new { data = listaDtoSaida }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateSaida()
        {
            var DtoSaida = new DtoSaida();
            return PartialView("~/Views/Movimentacoes/FormularioSaida.cshtml", DtoSaida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSaida(DtoSaida dtoSaida)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            try
            {
                //conversor
                var movimentacao = new Movimentacao
                {
                    Descricao = dtoSaida.Descricao,
                    DataCadastro = DateTime.Parse(dtoSaida.DataCadastro),
                    Valor = dtoSaida.Valor,
                    Tipo = dtoSaida.Tipo
                };

                servico.Cadastrar(movimentacao);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSaida(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacaoSaida = servico.ConsultePorId(id);

            var dtoSaida= new DtoSaida
            {
                Id = movimentacaoSaida.Id,
                Descricao = movimentacaoSaida.Descricao,
                DataCadastro = movimentacaoSaida.DataCadastro.ToShortDateString(),
                Valor = movimentacaoSaida.Valor
            };

            return PartialView("~/Views/Movimentacoes/FormularioSaida.cshtml", dtoSaida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSaida(DtoSaida dtoSaida)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            try
            {
                var movimentacaoSaida = new Movimentacao
                {
                    Id = dtoSaida.Id,
                    Descricao = dtoSaida.Descricao,
                    DataCadastro = DateTime.Parse(dtoSaida.DataCadastro),
                    Valor = dtoSaida.Valor,
                    Tipo = dtoSaida.Tipo
                };

                servico.Atualizar(movimentacaoSaida);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }
    }
}