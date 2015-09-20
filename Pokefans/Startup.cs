// Copyright 2015 the pokefans authors. See copying.md for legal info.
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Pokefans.Security;

[assembly: OwinStartupAttribute(typeof(Pokefans.Startup))]
namespace Pokefans
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ApplicationUserManager.dataProtectionProvider = app.GetDataProtectionProvider();
            ConfigureAuth(app);
        }
    }
}
