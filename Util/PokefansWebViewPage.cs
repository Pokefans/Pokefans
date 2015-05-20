using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
    }

    public abstract class PokefansWebViewPage : PokefansWebViewPage<dynamic> { }
}
