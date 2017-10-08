using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Groll.Schule.SchuleDBWeb.Startup))]
namespace Groll.Schule.SchuleDBWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
