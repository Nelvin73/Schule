using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchuleDB_Web.Startup))]
namespace SchuleDB_Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
