// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;

namespace Pokefans.Util
{
    public interface IBreadcrumbs
    {
        void Add(string text);
        void Add(string text, string controller, string action);
        void Add(string text, string controller, string action, object routeValues);
        List<Breadcrumb> Current { get; }
    }
}
