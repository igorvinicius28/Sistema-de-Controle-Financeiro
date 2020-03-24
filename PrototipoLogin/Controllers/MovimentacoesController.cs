using FCMoney.Dominio.Entidades.Movimentacoes;
using FCMoney.Dominio.Enumeradores;
using FCMoney.Dominio.Interfaces.Servicos;
using FCMoney.Repositorio.Repositorios.RepositorioMovimentacoes;
using FCMoney.Repositorio.Repositorios.Util;
using FCMoney.Repositorio.Servicos.Servicos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using NLog.Fluent;
using PrototipoLogin.Identity;
using PrototipoLogin.Models.Movimentacoes;
using Serilog;
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



        /************************* MOVIMENTACOES DE ENTRADA ****************************/

        // GET: Movimentacoes
        public ActionResult CadastrarEntrada()
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            var lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.ENTRADA).ToList();
            var listaDtoEntrada = new List<DtoEntrada>();
            lista.ForEach(x =>
            {
                var dtoEntrada = new DtoEntrada()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro,
                    Descricao = x.Descricao,
                    Tipo = EnumTipo.ENTRADA,
                    Valor = x.Valor
                };
                listaDtoEntrada.Add(dtoEntrada);
            });


            return View(listaDtoEntrada);
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CadastrarEntrada(DtoEntrada model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            //CONVERSOR
              Movimentacao movimentacao = new Movimentacao();
              movimentacao.Descricao = model.Descricao;
              movimentacao.Valor = model.Valor;
              movimentacao.DataCadastro = model.DataCadastro;
              movimentacao.Tipo = EnumTipo.ENTRADA;
             // movimentacao.FoiPaga = true;
              movimentacao.UsuarioId = user.Id;

            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            servico.Cadastrar(movimentacao);

            return View(model);
        }

        public ActionResult Create()
        {
            var DtoEntrada = new DtoEntrada();
            return PartialView("~/Views/Movimentacoes/FormularioEntrada.cshtml", DtoEntrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEntrada(DtoEntrada dtoEntrada)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            try
            {
                //conversor
                var movimentacao = new Movimentacao
                {
                    Descricao = dtoEntrada.Descricao,
                    DataCadastro = dtoEntrada.DataCadastro,
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

        public ActionResult Edit(int id, EnumTipo tipo)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacao = servico.ConsultePorId(id);

            if (tipo == EnumTipo.ENTRADA) {

                var dtoEntrada = new DtoEntrada
                {
                    Id = movimentacao.Id,
                    Descricao = movimentacao.Descricao,
                    DataCadastro = movimentacao.DataCadastro,
                    Valor = movimentacao.Valor
                };

                return PartialView("~/Views/Movimentacoes/FormularioEntrada.cshtml", dtoEntrada);
            }
            else
            {
                var dtoSaida = new DtoSaida
                {
                    Id = movimentacao.Id,
                    Descricao = movimentacao.Descricao,
                    DataCadastro = movimentacao.DataCadastro,
                    Valor = movimentacao.Valor
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
                    DataCadastro = dtoEntrada.DataCadastro,
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

        public ActionResult excluirs(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacaoEntrada = servico.ConsultePorId(id);

            var dtoEntrada = new DtoEntrada
            {
                Id = movimentacaoEntrada.Id,
                Descricao = movimentacaoEntrada.Descricao,
                DataCadastro = movimentacaoEntrada.DataCadastro,
                Valor = movimentacaoEntrada.Valor
            };

            return PartialView("~/Views/Movimentacoes/ConfirmacaoExcluirModal.cshtml", dtoEntrada);
        }

        public ActionResult excluir(int id)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());

            var movimentacaoEntrada = servico.ConsultePorId(id);

            servico.Excluir(movimentacaoEntrada);

            return Json(new { Resultado = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Listar(string nome)
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            var lista = new List<Movimentacao>();

            try
            {
                if (string.IsNullOrEmpty(nome))
                    lista = servico.ConsulteLista();
                else
                    lista = servico.ConsulteLista().Where(m => m.Descricao.Contains(nome)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            var listaDtoEntrada = new List<DtoEntrada>();
            lista.ForEach(x =>
            {
                var dtoEntrada = new DtoEntrada()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro,
                    Descricao = x.Descricao,
                    Tipo = EnumTipo.ENTRADA,
                    Valor = x.Valor
                };
                listaDtoEntrada.Add(dtoEntrada);
            });

            return PartialView("~/Views/Cliente/CadastrarEntrada.cshtml", listaDtoEntrada);
        }

        /********************************************** MOVIMENTACOES DE SAIDA ***************************************/

        // GET: Movimentacoes
        public ActionResult CadastrarSaida()
        {
            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            var lista = servico.ConsulteLista().Where(x => x.Tipo == EnumTipo.SAIDA).ToList();
            var listaDtoSaida= new List<DtoSaida>();
            lista.ForEach(x =>
            {
                var dtoSaida = new DtoSaida()
                {
                    Id = x.Id,
                    DataCadastro = x.DataCadastro,
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    Valor = x.Valor
                };
                listaDtoSaida.Add(dtoSaida);
            });

            return View(listaDtoSaida);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CadastrarSaida(DtoEntrada model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            //CONVERSOR
            Movimentacao movimentacao = new Movimentacao();
            movimentacao.Descricao = model.Descricao;
            movimentacao.Valor = model.Valor;
            movimentacao.DataCadastro = model.DataCadastro;
            movimentacao.Tipo = model.Tipo;
            movimentacao.UsuarioId = user.Id;

            ServicoDeMovimentacaoImpl servico = new ServicoDeMovimentacaoImpl(new RepositorioMovimentacoes());
            servico.Cadastrar(movimentacao);

            return View(model);
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
                    DataCadastro = dtoSaida.DataCadastro,
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
                DataCadastro = movimentacaoSaida.DataCadastro,
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
                    DataCadastro = dtoSaida.DataCadastro,
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