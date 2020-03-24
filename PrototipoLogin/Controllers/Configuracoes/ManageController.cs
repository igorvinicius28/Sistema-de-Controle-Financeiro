using PrototipoLogin.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PrototipoLogin.Models.Configuracoes;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Dominio.Entidades;
using FCMoney.Repositorio.Servicos.Servicos;
using PrototipoLogin.Models.Usuario;
using FCMoney.Repositorio.Repositorios.RepositorioUsuario;

namespace PrototipoLogin.Controllers.Configuracoes
{
    [Authorize]
    public class ManageController : Controller
    {

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
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

        //
        // GET: /Account/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "A senha foi alterada."
                : message == ManageMessageId.SetPasswordSuccess ? "A senha foi enviada."
                : message == ManageMessageId.SetTwoFactorSuccess ? "A segunda validação foi enviada."
                : message == ManageMessageId.Error ? "Ocorreu um erro."
                : message == ManageMessageId.AddPhoneSuccess ? "O Telefone foi adicionado."
                : message == ManageMessageId.RemovePhoneSuccess ? "O Telefone foi removido."
                : "";

            var model = new DtoConfiguracoes
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(DtoAlteracaoDeSenha model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public async Task<ActionResult> DadosPessoais()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            ServicoDeDadosPessoaisImpl servico = new ServicoDeDadosPessoaisImpl(new RepositorioDadosPessoais());
            var dadosPessoais = servico.ConsultePorId(user.Id);
            DtoDadosPessoais model = new DtoDadosPessoais();
            if (dadosPessoais != null)
            {
                //CONVERSOR
                model.Nome = dadosPessoais.Nome;
                model.CPF = dadosPessoais.CPF;

                model.Telefone = dadosPessoais.Telefone;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DadosPessoais(DtoDadosPessoais model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            DadosPessoais dadosPessoais = new DadosPessoais();
            dadosPessoais.Nome = model.Nome;
            dadosPessoais.Telefone = model.Telefone;
            dadosPessoais.CPF = model.CPF;
            dadosPessoais.UsuarioId = user.Id;

            ServicoDeDadosPessoaisImpl servico = new ServicoDeDadosPessoaisImpl(new RepositorioDadosPessoais());
            servico.Atualizar(dadosPessoais);

            return View();
        }


        // UTILITARIOS

        #region Utilitarios
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            var clientKey = Request.Browser.Type;
            await UserManager.SignInClientAsync(user, clientKey);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

    }
}