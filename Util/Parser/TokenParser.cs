// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System.Collections.Generic;
using System.ComponentModel;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Base class for all token parsers
    /// </summary>
    public abstract class TokenParser
    {
        /// <summary>
        /// Parsed contents
        /// </summary>
        public string Contents { get; protected set; }

        /// <summary>
        /// parser configuration
        /// </summary>
        protected ParserConfiguration Configuration { get; set; }

        /// <summary>
        /// Create a new TokenParser instance
        /// </summary>
        /// <param name="config"></param>
        protected TokenParser(ParserConfiguration config)
        {
            Configuration = config;
            Contents = string.Empty;
        }

        /// <summary>
        /// Process a new character.
        /// </summary>
        /// <param name="character"></param>
        /// <returns><value>true</value> if the token is finished by <paramref name="character"/>, otherwise <value>false</value>.</returns>
        public bool ProcessCharacter(char character)
        {
            TokenParser parser;
            return ProcessCharacter(character, out parser);
        }

        /// <summary>
        /// Process a new character.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="nextTokenParser">Will be set to a new TokenParser instance if the end is reached.</param>
        /// <returns><value>true</value> if the token is finished by <paramref name="character"/>, otherwise <value>false</value>.</returns>
        public abstract bool ProcessCharacter(char character, out TokenParser nextTokenParser);

        /// <summary>
        /// Creates a new Token containing the gathered information.
        /// </summary>
        /// <returns>New Token (or subclass) instance.</returns>
        public abstract Token CreateToken();
    }
}
