// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Represents one token containing plain text
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Contents
        /// </summary>
        public string Contents { get; set; }
    }

    /// <summary>
    /// Represents one token containing an inside code
    /// </summary>
    public class InsideCodeToken : Token
    {
        /// <summary>
        /// Inside code identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Collection of parameters
        /// </summary>
        public string[] Parameters { get; set; }
    }

    /// <summary>
    /// Represents one token containing an opening bb code
    /// </summary>
    public class BbCodeToken : Token
    {
        /// <summary>
        /// Bb code identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// First unnamed parameter or null
        /// </summary>
        public string UnnamedParameter { get; set; }

        /// <summary>
        /// Map of named parameters
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }

    /// <summary>
    /// Represents one token containing a closing bb code
    /// </summary>
    public class BbCodeEndToken : Token
    {
        /// <summary>
        /// Bb code identifier
        /// </summary>
        public string Identifier { get; set; }
    }
}
