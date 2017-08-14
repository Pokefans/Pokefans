// Copyright 2015-2016 the pokefans-core authors. See copying.md for legal info.
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

        public string ActiveMenuKey {
            get { return ViewBag.ActiveMenuKey; }
            set { ViewBag.ActiveMenuKey = value; }
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

        private User _user;
        public User CurrentUser
        {
            get
            {
                // there can be no user if we are not authenticated.
                if(!User.Identity.IsAuthenticated) 
                {
                    return null;
                }

                // Let's lookup the user once per request.
                if (_user == null)
                {
                    ApplicationUserManager mgr = DependencyResolver.Current.GetService<ApplicationUserManager>();

                    _user = mgr.FindByNameAsync(User.Identity.Name).Result;
                }
                return _user;
            }
        }

        // This is probably the wrong place in the pipeline for this, but unfortunately
        // I don't know of a better place that comes in this handy. Suggestions welcome.

        private int? _notifications;
        public int UnreadNotifications 
        {
            get {
                if(CurrentUser == null) 
                {
                    return 0;
                }
                if (_notifications == null)
                {
                    var db = DependencyResolver.Current.GetService<Entities>();
                    _notifications = db.UserNotifications.Where(x => x.UserId == CurrentUser.Id).Count();
                }
                return _notifications.Value;
            }
        }

        private int? _messages;
        public int UnreadMessages 
        {
            get {
                if(CurrentUser == null)
                {
                    return 0;
                }
                if(_messages == null) 
                {
                    var db = DependencyResolver.Current.GetService<Entities>();
                    _messages = db.PrivateMessagesInbox.Where(x => x.UserToId == CurrentUser.Id && x.Read == false).Count();
                }
                return _messages.Value;
            }
        }

        public string isActive(string index)
        {
            return ActiveMenuKey == index ? "active" : "";
        }
    }

    public abstract class PokefansWebViewPage : PokefansWebViewPage<dynamic> { }
}
