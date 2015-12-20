using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoShineMP.Startup))]
namespace DoShineMP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
