using FCMoney.Dominio.Contratos;
using FCMoney.Dominio.Entidades;
using FCMoney.Dominio.Entidades.Autenticacao;
using FCMoney.Repositorio.Repositorios.RepositorioUsuario;
using FCMoney.Repositorio.Servicos.Servicos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PrototipoLogin.Identity;
using PrototipoLogin.Models.Usuario;
using System.Collections;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrototipoLogin.Controllers
{
    [Authorize]
    public class AutenticacaoController : Controller
    {
       // private readonly IUsuarioRepositorio _repositorioUsuario;

        public AutenticacaoController()
        {
        //    getRepositorioUsuario();
        }
         
        public AutenticacaoController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        // Definindo a instancia UserManager presente no request.
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
            
        // Definindo a instancia SignInManager presente no request.
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Autenticacao
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult esquecerSenha2()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> esquecerSenha(DtoRecuperarSenha model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("recuperarSenha", "Autenticacao", new { UserId = user.Id, code = code }, protocol: Request.Url.Scheme);
                                             
                await UserManager.SendEmailAsync(user.Id, "Reset Password",
                "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("Login");
            }


            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult recuperarSenha(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // GET: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(DtoRecuperarSenha model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Não revelar se o usuario nao existe ou nao esta confirmado
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Autenticacao");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        /*
        //
        // GET: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string email)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        */

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(DtoLogin model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindAsync(model.Email, model.Password);
            if (user != null && !user.EmailConfirmed)
            {
                return View("DisplayEmailNaoConfirmado",model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Login ou Senha incorretos.");
                    return View(model);
            }
        }

        // CADASTRO

        [AllowAnonymous]
        public async Task<ActionResult> ReconfirmarEmail2(DtoLogin login)
        {
            var user = await UserManager.FindByNameAsync(login.Email);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Autenticacao", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", callbackUrl);
            ViewBag.Link = callbackUrl;

            return View("DisplayEmail");
        }

        // CADASTRO

        // GET: Autenticacao
        [AllowAnonymous]
        public ActionResult Cadastrar()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastrar(DtoCadastro model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Autenticacao", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", callbackUrl);
                    //await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", "");
                    ViewBag.Link = callbackUrl;

                    // CADASTRO DE DADOS PESSOAIS (TIRAR DAQUI DEPOIS)
                    ServicoDeDadosPessoaisImpl servico = new ServicoDeDadosPessoaisImpl(new RepositorioDadosPessoais());
                    DadosPessoais dadosPessoais = new DadosPessoais();
                    dadosPessoais.Nome = model.Nome;
                    dadosPessoais.Sobrenome = model.Sobrenome;
                    dadosPessoais.UsuarioId = user.Id;
                    servico.Cadastrar(dadosPessoais);
                    //FIM CADASTRO DADOS PESSOAIS

                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // No caso de falha, reexibir a view. 
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOutClient(int clientId)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var client = user.Clients.SingleOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                user.Clients.Remove(client);
            }
            UserManager.Update(user);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Autenticacao");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOutEverywhere()
        {
            UserManager.UpdateSecurityStamp(User.Identity.GetUserId());
            await SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //====================== UTILITARIOS ========================//

        private async Task SignOutAsync()
        {
            var clientKey = Request.Browser.Type;
            var user = UserManager.FindById(User.Identity.GetUserId());
            await UserManager.SignOutClientAsync(user, clientKey);
            AuthenticationManager.SignOut();
        }


        private void AddErrors(IdentityResult result)
        {
            string msg = "";

            foreach (var error in result.Errors)
            {
                IEnumerator enumerator = result.Errors.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    msg = (string) enumerator.Current;
                }

                if(msg.Contains("is already taken."))
                {
                    if (msg.Contains("Email"))
                    {
                        msg = "Já existe uma conta vinculada a esse email";
                    }
                    if (msg.Contains("Name"))
                    {
                        continue;
                    }
                }
                
                ModelState.AddModelError("", msg);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            var clientKey = Request.Browser.Type;
            await UserManager.SignInClientAsync(user, clientKey);
            // Zerando contador de logins errados.
            await UserManager.ResetAccessFailedCountAsync(user.Id);

            // Coletando Claims externos (se houver)
            ClaimsIdentity ext = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn
                (
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    // Criação da instancia do Identity e atribuição dos Claims
                    await user.GenerateUserIdentityAsync(UserManager, ext)
                );
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

    }
}