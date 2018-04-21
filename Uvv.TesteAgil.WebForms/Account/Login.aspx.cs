using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Uvv.TesteAgil.WebForms.Models;
using Uvv.TesteAgil.Dados.DAL;
using System.Linq;
using Uvv.TesteAgil.WebForms.Util;

namespace Uvv.TesteAgil.WebForms.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            //// Ativar quando tiver a confirmação da conta habilitada para a funcionalidade de redefinição de senha
            ////ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                bool autorizado = false;
                var membro = new MembroDAL().ObterMembroPorEmailESenha(Email.Text, Password.Text);

                if (membro != null) autorizado = true;

                if (autorizado)
                {
                    // Valide a senha de usuário
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                    var result = signinManager.PasswordSignIn(Email.Text, Email.Text, RememberMe.Checked, shouldLockout: false);

                    switch (result)
                    {
                        //Se encontrar o usuário de acesso deve atualizar o nome no caso de mudança
                        case SignInStatus.Success:
                            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                            break;
                        case SignInStatus.LockedOut:
                            Response.Redirect("/Account/Lockout");
                            break;
                        case SignInStatus.RequiresVerification:
                            Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                            break;
                        //Usuário autorizado e não cadastrado será cadastrado como usuário de controle de acesso
                        case SignInStatus.Failure:
                            var user = new ApplicationUser() { UserName = membro.Email, Email = membro.Email, Nome = membro.Nome, MembroId = membro.MembroId.ToString() };
                            IdentityResult identityResult = manager.Create(user, Email.Text);
                            if (identityResult.Succeeded)
                            {
                                result = signinManager.PasswordSignIn(Email.Text, Email.Text, RememberMe.Checked, shouldLockout: false);
                                if (result == SignInStatus.Success)
                                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                else
                                {
                                    FailureText.Text = identityResult.Errors.FirstOrDefault();
                                    ErrorMessage.Visible = true;
                                }
                            }
                            else
                            {
                                FailureText.Text = identityResult.Errors.FirstOrDefault();
                                ErrorMessage.Visible = true;
                            }
                            break;
                    }
                }
            }

        }
    }
}