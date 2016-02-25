// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokefans.Data;

namespace Pokefans.Security
{
    public class WebSecurity
    {
        [Obsolete]
        public static User CurrentUser { get; set; }
    }
}
