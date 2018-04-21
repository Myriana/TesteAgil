using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;

namespace Uvv.TesteAgil.WebForms
{
    public class PaginaBase : Page
    {
        public string GetUserName()
        {
            var user = Page.User as ClaimsPrincipal;

            string nmeUsuario = string.Empty;

            var claimNome = user.Claims.FirstOrDefault(c => c.Type == "nmeUsuario");
            if (claimNome != null)
                nmeUsuario = claimNome.Value;

            return nmeUsuario;
        }

        public int GetUserId()
        {
            var user = Page.User as ClaimsPrincipal;

            int numId = 0;

            var claimId = user.Claims.FirstOrDefault(c => c.Type == "numId");
            if (claimId != null)
            {
                int.TryParse(claimId.Value, out numId);
            }

            return numId;
        }

        public string GetUserEmail()
        {
            var user = Page.User as ClaimsPrincipal;

            string nmeEmail = string.Empty;

            var claimEmail = user.Claims.FirstOrDefault(c => c.Type == "nmeEmail");
            if (claimEmail != null)
                nmeEmail = claimEmail.Value;

            return nmeEmail;
        }

        public void ValidarSessao()
        {
            if (!Request.IsAuthenticated)
                Response.Redirect("/Account/Login");
        }
    }
}