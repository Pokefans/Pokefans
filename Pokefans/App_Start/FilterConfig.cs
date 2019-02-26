// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Web;
using System.Web.Mvc;

namespace Pokefans
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
