using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uvv.TesteAgil.Web.Startup))]
namespace Uvv.TesteAgil.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
