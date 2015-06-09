// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Pokefans.Data;
using Pokefans.Security;

namespace Pokefans.Util
{
    public abstract class PokefansWebViewPage<TModel> : WebViewPage<TModel>
    {
        public override void InitHelpers()
        {
            base.InitHelpers();
            Breadcrumbs = DependencyResolver.Current.GetService<IBreadcrumbs>();
        }

        public IBreadcrumbs Breadcrumbs
        {
            get;
            private set;
        }

        public string SiteTitle
        {
            get
            {
                return ViewBag.SiteTitle;
            }
            set
            {
                ViewBag.SiteTitle = value;
            }
        }

        public string SiteHeader
        {
            get
            {
                return ViewBag.SiteHeader;
            }
            set
            {
                ViewBag.SiteHeader = value;
            }
        }
        public string SiteDescription
        {
            get
            {
                return ViewBag.SiteDescription;
            }
            set
            {
                ViewBag.SiteDescription = value;
            }
        }

        public User CurrentUser
        {
            get
            {
                ApplicationUserManager mgr = DependencyResolver.Current.GetService<ApplicationUserManager>();

                return mgr.FindByNameAsync(User.Identity.Name).Result;
            }
        }
    }

    public abstract class PokefansWebViewPage : PokefansWebViewPage<dynamic> { }
}
