// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Token parser for inside codes
    /// </summary>
    public class InsideCodeTokenParser : TokenParser
    {
        /// <summary>
        /// Collection of characters that are allowed as part of an identifier
        /// </summary>
        private const string IdentifierCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        /// <summary>
        /// Collection of characters that are not allowed inside an inside code
        /// </summary>
        private const string DisallowedCharacters = "\n\r";

        /// <summary>
        /// Inside code identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// List of given parameters
        /// </summary>
        public IList<string> Parameters { get; private set; }

        /// <summary>
        /// True if the code is valid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// True if the identifier has been completely read yet.
        /// </summary>
        private bool _identifierRead;

        /// <summary>
        /// Creates a new InsideCodeTokenParser instance
        /// </summary>
        public InsideCodeTokenParser(ParserConfiguration config)
            : base(config)
        {
            _identifierRead = false;
            Identifier = string.Empty;
            Parameters = new List<string>();
            IsValid = false;
        }

        /// <summary>
        /// Process a new character.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="nextTokenParser">Will be set to a new TokenParser instance if the end is reached.</param>
        /// <returns><value>true</value> if the token is finished by <paramref name="character"/>, otherwise <value>false</value>.</returns>
        public override bool ProcessCharacter(char character, out TokenParser nextTokenParser)
        {
            Contents += character;

            if (character == '}')
            {
                IsValid = true;
                nextTokenParser = null;
                return true;
            }

            // Ignore opening bracket
            if (character == '{' && string.IsNullOrEmpty(Identifier))
            {
                nextTokenParser = null;
                return true;
            }

            if (DisallowedCharacters.Contains(character))
            {
                throw new InvalidOperationException(string.Format("Unexpected character {0} inside IC.", character));
            }

            if (!_identifierRead)
            {
                if (character == ':')
                {
                    _identifierRead = true;
                    Parameters.Add(string.Empty);
                }
                else if (IdentifierCharacters.Contains(character))
                {
                    Identifier += character;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unexpected character {0} inside IC, expected identifier.", character));
                }
            }
            else
            {
                if (character == ';')
                {
                    Parameters.Add(string.Empty);
                }
                else
                {
                    Parameters[Parameters.Count - 1] += character;
                }
            }

            nextTokenParser = null;
            return false;
        }

        /// <summary>
        /// Creates a new Token containing the gathered information.
        /// </summary>
        /// <returns>New Token (or subclass) instance.</returns>
        public override Token CreateToken()
        {
            if (IsValid)
            {
                return new InsideCodeToken
                {
                    Identifier = Identifier,
                    Parameters = Parameters.ToArray(),
                    Contents = Contents
                };
            }

            return new Token
            {
                Contents = Contents
            };
        }
    }
}
