using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uvv.TesteAgil.WebForms.Startup))]
namespace Uvv.TesteAgil.WebForms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
