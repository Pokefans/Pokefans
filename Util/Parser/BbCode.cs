// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Linq;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Basic BbCode class
    /// </summary>
    public abstract class BbCode
    {
        /// <summary>
        /// Convert BbCode to HTML
        /// </summary>
        /// <param name="token">Parsed token</param>
        /// <param name="content">Code content</param>
        /// <returns>Formatted code</returns>
        public abstract string Format(BbCodeToken token, string content);
    }

    /// <summary>
    /// Replacement based BbCode
    /// </summary>
    public class BasicBbCode : BbCode
    {
        /// <summary>
        /// Parameterless template
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Parameterized template
        /// </summary>
        public string TemplateParameterized { get; set; }

        /// <summary>
        /// Convert BbCode to HTML
        /// </summary>
        /// <param name="token">Parsed token</param>
        /// <param name="content">Code content</param>
        /// <returns>Formatted code</returns>
        public override string Format(BbCodeToken token, string content)
        {
            return token.UnnamedParameter == null ? String.Format(Template, content) : String.Format(TemplateParameterized ?? Template, content, token.UnnamedParameter);
        }
    }

    public class ParameterizedBbCode : BbCode
    {
        /// <summary>
        /// Parameterized template
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// List of parameters
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// Convert BbCode to HTML
        /// </summary>
        /// <param name="token">Parsed token</param>
        /// <param name="content">Code content</param>
        /// <returns>Formatted code</returns>
        public override string Format(BbCodeToken token, string content)
        {
            // Map parameter values to indices
            var values = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
            {
                string value;
                token.Parameters.TryGetValue(Parameters[i], out value);

                values[i] = value;
            }

            return string.Format(Template, values);
        }
    }
}
