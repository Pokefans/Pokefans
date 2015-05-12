using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pokefans.Startup))]
namespace Pokefans
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
