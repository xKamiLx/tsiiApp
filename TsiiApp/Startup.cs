using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TsiiApp.Startup))]
namespace TsiiApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
