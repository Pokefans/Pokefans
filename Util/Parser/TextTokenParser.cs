// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Token parser for plain text
    /// </summary>
    public class TextTokenParser : TokenParser
    {
        /// <summary>
        /// Creates a new TextTokenParser instance
        /// </summary>
        public TextTokenParser(ParserConfiguration config) : base(config)
        {

        }

        /// <summary>
        /// Process a new character.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="nextTokenParser">Will be set to a new TokenParser instance if the end is reached.</param>
        /// <returns><value>true</value> if the token is finished by <paramref name="character"/>, otherwise <value>false</value>.</returns>
        public override bool ProcessCharacter(char character, out TokenParser nextTokenParser)
        {
            if (Configuration.TokenOpeningCharacters.ContainsKey(character))
            {
                var parserType = Configuration.TokenOpeningCharacters[character];
                nextTokenParser = (TokenParser) Activator.CreateInstance(parserType, ParserConfiguration.Default);
                nextTokenParser.ProcessCharacter(character);

                return true;
            }

            Contents += character;

            nextTokenParser = null;
            return false;
        }

        /// <summary>
        /// Creates a new Token containing the gathered information.
        /// </summary>
        /// <returns>New Token (or subclass) instance.</returns>
        public override Token CreateToken()
        {
            var contents = Contents;
            if (Configuration.EscapeHtml)
            {
                contents = System.Web.HttpUtility.HtmlEncode(contents);
            }
            if (Configuration.NewlineToHtml)
            {
                contents = contents.Replace("\n", "<br>");
            }

            return new Token
            {
                Contents = contents
            };
        }
    }
}
