using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OgsysCRM.Startup))]
namespace OgsysCRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
