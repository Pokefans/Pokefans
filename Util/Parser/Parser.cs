// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Stack machine based parser for parsing markup
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// parser configuration
        /// </summary>
        protected ParserConfiguration Configuration;

        /// <summary>
        /// stack
        /// </summary>
        protected Stack<Token> Stack;

        /// <summary>
        /// current token
        /// </summary>
        protected TokenParser CurrentTokenParser;

        /// <summary>
        /// Collection of errors found while parsing
        /// </summary>
        public ICollection<string> Errors { get; private set; }

        /// <summary>
        /// Creates a new Parser object with the default configuration.
        /// </summary>
        public Parser() : this(ParserConfiguration.Default) { }

        /// <summary>
        /// Creates a new Parser with a given configuration.
        /// </summary>
        /// <param name="configuration">ParserConfiguration object containing all settings.</param>
        public Parser(ParserConfiguration configuration)
        {
            Configuration = configuration;
            Errors = new List<string>();
        }

        /// <summary>
        /// Resets the state of the parser.
        /// </summary>
        protected void Reset()
        {
            Stack = new Stack<Token>();
            CurrentTokenParser = new TextTokenParser(Configuration);
            Errors.Clear();
        }

        /// <summary>
        /// Parse a string.
        /// </summary>
        /// <param name="contents">String containing markup</param>
        /// <returns>Parsed string with replaced markup</returns>
        public string Parse(string contents)
        {
            // Reset parser state and remove any leftovers
            Reset();

            // Start tokenizing
            foreach (var character in contents)
            {
                TokenParser nextTokenParser;

                // If the character does not finish the token continue with the next one
                if (! CurrentTokenParser.ProcessCharacter(character, out nextTokenParser))
                    continue;

                Token token = CurrentTokenParser.CreateToken();

                // If there is a closing tag we have to find the start tag
                if (token is BbCodeEndToken)
                {
                    var identifier = (token as BbCodeEndToken).Identifier;
                    var tokenList = new List<Token>();

                    while (Stack.Count > 0)
                    {
                        var currentToken = Stack.Pop();

                        if (currentToken is BbCodeToken)
                        {
                            var name = (currentToken as BbCodeToken).Identifier;

                            if (name == identifier)
                            {
                                tokenList.Reverse();
                                var content = tokenList.Aggregate(string.Empty, (current, t) => current + t.Contents);

                                Stack.Push(new Token
                                {
                                    Contents = ParseBbCode(currentToken as BbCodeToken, content)
                                });
                                break;
                            }

                            throw new InvalidOperationException(String.Format("Unexpected opening tag {0}, expected {1}.", name, identifier));
                        }
                        else if (currentToken is InsideCodeToken)
                        {
                            Stack.Push(new Token
                            {
                                Contents = ParseInsideCode(currentToken as InsideCodeToken)
                            });
                        }
                        else if (currentToken is BbCodeEndToken)
                        {
                            throw new InvalidOperationException(String.Format("Unexpected closing tag {0}.", (currentToken as BbCodeEndToken).Identifier));
                        }
                        else
                        {
                            tokenList.Add(currentToken);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(token.Contents))
                {
                    Stack.Push(token);
                }

                CurrentTokenParser = nextTokenParser ?? new TextTokenParser(Configuration);
            }

            var result = string.Empty;
            while (Stack.Count > 0)
            {
                var currentToken = Stack.Pop();
                string content;

                if (currentToken is InsideCodeToken)
                {
                    content = ParseInsideCode(currentToken as InsideCodeToken);
                }
                else
                {
                    content = currentToken.Contents;
                }

                result = content + result;
            }

            return result;
        }

        /// <summary>
        /// Parse a bb code and retun HTML markup
        /// </summary>
        /// <param name="bbCodeToken"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string ParseBbCode(BbCodeToken bbCodeToken, string content)
        {
            if (Configuration.EnableBbCodes && Configuration.BbCodes.ContainsKey(bbCodeToken.Identifier))
            {
                // TODO: Parse all parameters
                if (!string.IsNullOrEmpty(bbCodeToken.UnnamedParameter))
                {
                    var parser = new Parser(Configuration);
                    bbCodeToken.UnnamedParameter = parser.Parse(bbCodeToken.UnnamedParameter);
                }

                return Configuration.BbCodes[bbCodeToken.Identifier].Format(bbCodeToken, content);
            }

            return string.Format("{0}{1}[/{2}]", bbCodeToken.Contents, content, bbCodeToken.Identifier);
        }

        /// <summary>
        /// Parse an inside code to text and return HTML markup
        /// </summary>
        /// <param name="insideCodeToken"></param>
        /// <returns></returns>
        protected string ParseInsideCode(InsideCodeToken insideCodeToken)
        {
            if (Configuration.EnableInsideCodes && Configuration.InsideCodes.ContainsKey(insideCodeToken.Identifier))
            {
                return Configuration.InsideCodes[insideCodeToken.Identifier].Format(insideCodeToken);
            }

            return insideCodeToken.Contents;
        }
    }
}
