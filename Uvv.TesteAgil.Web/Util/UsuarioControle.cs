using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uvv.TesteAgil.Web.Models;

namespace Uvv.TesteAgil.Web.Util
{
    public class UsuarioControle
    {
        public ApplicationUser ObterNomeUsuarioLogado(string userId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(userId);

            return user;
        }
    }
}