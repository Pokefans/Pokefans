// Copyright 2015-2016 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Pokefans.Data;
using Pokefans.Security;
using Pokefans.Caching;
using Pokefans.Util;
using Amib.Threading;
using Pokefans.Util.Comments;

namespace Pokefans.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<Entities>(new PerResolveLifetimeManager());
            container.RegisterType(typeof(IUserStore<User, int>), typeof(UserStore));
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory( c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IRoleStore<Role, int>, RoleStore>(new InjectionConstructor(typeof(Entities)));
            container.RegisterType<SmartThreadPool>(new InjectionFactory(c => new SmartThreadPool()));
            container.RegisterType<CommentManager>(new PerResolveLifetimeManager());

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
