// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Token parser for bb codes
    /// </summary>
    public class BbCodeTokenParser : TokenParser
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
        /// Bb code identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// First unnamed parameter or null
        /// </summary>
        public string UnnamedParameter { get; private set; }

        /// <summary>
        /// Map of named parameters
        /// </summary>
        public Dictionary<string, string> Parameters { get; private set; }

        /// <summary>
        /// True if the tag is a closing one
        /// </summary>
        public bool IsEndTag { get; private set; }

        /// <summary>
        /// True if the code is valid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// True if the identifier has been completely read yet.
        /// </summary>
        private bool _identifierRead;

        /// <summary>
        /// Name of the current parameter
        /// </summary>
        private string _parameterName;

        /// <summary>
        /// True if a parameter name has been completely read yet.
        /// </summary>
        private bool _parameterRead;

        /// <summary>
        /// True if a parameter in enclosed in quotes
        /// </summary>
        private bool _inQuotedValue;

        /// <summary>
        /// Creates a new BbCodeTokenParser instance
        /// </summary>
        public BbCodeTokenParser(ParserConfiguration config)
            : base(config)
        {
            Identifier = string.Empty;
            UnnamedParameter = null;
            Parameters = new Dictionary<string, string>();
            IsEndTag = false;
            IsValid = false;
            _identifierRead = false;
            _parameterName = null;
            _parameterRead = false;
            _inQuotedValue = false;
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

            // Special handling for quoted parameters has to be done before everything else.
            if (_identifierRead && _parameterRead && _inQuotedValue)
            {
                if (character == '"')
                {
                    _inQuotedValue = false;
                }
                else
                {

                    if (_parameterName == null)
                    {
                        UnnamedParameter += character;
                    }
                    else
                    {
                        Parameters[_parameterName] += character;
                    }
                }

                nextTokenParser = null;
                return false;
            }

            // Handle closing bracket
            if (character == ']')
            {
                if (!_parameterRead && !string.IsNullOrEmpty(_parameterName))
                {
                    Parameters[_parameterName] = string.Empty;
                }

                IsValid = true;
                nextTokenParser = null;
                return true;
            }

            // Ignore opening bracket
            if (character == '[' && string.IsNullOrEmpty(Identifier))
            {
                nextTokenParser = null;
                return false;
            }

            // Check for invalid characters
            if (DisallowedCharacters.Contains(character))
            {
                throw new InvalidOperationException(string.Format("Unexpected character {0} inside bb code.", character));
            }

            // Check if the tag is a closing one
            if (string.IsNullOrEmpty(Identifier) && character == '/' && !IsEndTag)
            {
                IsEndTag = true;
                nextTokenParser = null;
                return false;
            }

            if (!_identifierRead)
            {
                // Read the identifier until a whitespace or equal sign
                if (character == '=' || character == ' ')
                {
                    _identifierRead = true;
                }
                else if (IdentifierCharacters.Contains(character))
                {
                    Identifier += character;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unexpected character {0} inside bb code, expected identifier.", character));
                }
            }

            // Closing tags must not contain parameters
            if (IsEndTag && _identifierRead)
            {
                throw new InvalidOperationException("Closing tags must not contain parameters.");
            }

            if (_identifierRead)
            {
                switch (character)
                {
                    // parse parameter value after an equal sign
                    case '=':
                        {
                            _parameterRead = true;
                            if (_parameterName == null)
                            {
                                if (!string.IsNullOrEmpty(UnnamedParameter))
                                {
                                    throw new InvalidOperationException("There must only be one unnamed parameter.");
                                }

                                UnnamedParameter = string.Empty;
                            }
                            else
                            {
                                Parameters[_parameterName] = string.Empty;
                            }
                            break;
                        }
                    // parse next parameter
                    case ' ':
                        {
                            _parameterRead = false;
                            _parameterName = null;
                            break;
                        }
                    // create parameter name/value
                    default:
                        {
                            if (_parameterRead)
                            {
                                if (character == '"')
                                {
                                    _inQuotedValue = true;
                                }
                                else
                                {
                                    if (_parameterName == null)
                                    {
                                        UnnamedParameter += character;
                                    }
                                    else
                                    {
                                        Parameters[_parameterName] += character;
                                    }
                                }
                            }
                            else
                            {
                                if (_parameterName == null)
                                {
                                    _parameterName = string.Empty;
                                }

                                if (IdentifierCharacters.Contains(character))
                                {
                                    _parameterName += character;
                                }
                                else
                                {
                                    throw new InvalidOperationException(string.Format("Unexpected character {0}, expected parameter name.", character));
                                }
                            }

                            break;
                        }
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
                if (IsEndTag)
                {
                    return new BbCodeEndToken
                    {
                        Identifier = Identifier,
                        Contents = Contents
                    };
                }
                
                return new BbCodeToken
                {
                    Identifier = Identifier,
                    Contents = Contents,
                    Parameters = Parameters,
                    UnnamedParameter = UnnamedParameter
                };
            }

            return new Token
            {
                Contents = Contents
            };
        }
    }
}
