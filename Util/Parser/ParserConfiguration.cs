// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;

namespace Pokefans.Util.Parser
{
    /// <summary>
    /// Object containing all settings for a parser
    /// </summary>
    public class ParserConfiguration
    {
        /// <summary>
        /// Determines whether or not BbCodes should be parsed.
        /// </summary>
        public bool EnableBbCodes { get; set; }

        /// <summary>
        /// Determines whether or not Inside Codes should be parsed.
        /// </summary>
        public bool EnableInsideCodes { get; set; }

        /// <summary>
        /// Map from characters to a token parser
        /// </summary>
        public Dictionary<char, Type> TokenOpeningCharacters { get; set; }

        /// <summary>
        /// Collection of available BbCodes
        /// </summary>
        public Dictionary<string, BbCode> BbCodes { get; set; }

        /// <summary>
        /// Collection of available BbCodes
        /// </summary>
        public Dictionary<string, InsideCode> InsideCodes { get; set; }

        /// <summary>
        /// Default configuration used by the parser
        /// </summary>
        public static ParserConfiguration Default
        {
            get
            {
                var configuration = new ParserConfiguration
                {
                    EnableBbCodes = true,
                    EnableInsideCodes = true,
                    TokenOpeningCharacters = new Dictionary<char, Type>
                    {
                        { '[', typeof(BbCodeTokenParser)},
                        { '{', typeof(InsideCodeTokenParser)},
                    },
                    BbCodes = new Dictionary<string, BbCode>(),
                    InsideCodes = new Dictionary<string, InsideCode>
                    {
                        { "link", new LinkInsideCode() },
                        { "url", new LinkInsideCode() },
                        { "img", new ImgInsideCode() }
                    }
                };

                configuration.BbCodes["b"] = new BasicBbCode
                {
                    Template = "<strong>{0}</strong>"
                };
                configuration.BbCodes["i"] = new BasicBbCode
                {
                    Template = "<em>{0}</em>"
                };
                configuration.BbCodes["u"] = new BasicBbCode
                {
                    Template = "<span style=\"text-decoration: underline;\">{0}</span>"
                };
                configuration.BbCodes["s"] = new BasicBbCode
                {
                    Template = "<del>{0}</del>"
                };
                configuration.BbCodes["h2"] = new BasicBbCode
                {
                    Template = "<h2>{0}</h2>"
                };
                configuration.BbCodes["q"] = new BasicBbCode
                {
                    Template = "<q>{0}</q>"
                };
                configuration.BbCodes["center"] = new BasicBbCode
                {
                    Template = "<center>{0}</center>"
                };
                configuration.BbCodes["hoch"] = new BasicBbCode
                {
                    Template = "<sup>{0}</sup>"
                };
                configuration.BbCodes["tief"] = new BasicBbCode
                {
                    Template = "<sub>{0}</sub>"
                };
                configuration.BbCodes["code"] = new BasicBbCode
                {
                    Template = "<code>{0}</code>"
                };
                configuration.BbCodes["quote"] = new BasicBbCode
                {
                    Template = "<blockquote>{0}</blockquote>",
                    TemplateParameterized = "<blockquote class=\"quote\"><strong>{1} hat geschrieben</strong>: {0}</blockquote>"
                };
                configuration.BbCodes["list"] = new BasicBbCode
                {
                    Template = "<ul>{0}</ul>"
                };
                configuration.BbCodes["*"] = new BasicBbCode
                {
                    Template = "<li>{0}</li>"
                };
                configuration.BbCodes["img"] = new BasicBbCode
                {
                    Template = "<img src=\"{0}\" alt=\"\" />"
                };
                configuration.BbCodes["sprite"] = new BasicBbCode
                {
                    Template = "<img src=\"http://files.pokefans.net/sprites/{0}.png\" alt=\"{0}\" />",
                    TemplateParameterized = "<img src=\"http://files.pokefans.net/sprites/{1}/{0}.png\" alt=\"{0}\" />"
                };
                configuration.BbCodes["url"] = new BasicBbCode
                {
                    Template = "<a href=\"{0}\">{0}</a>",
                    TemplateParameterized = "<a href=\"{1}\">{0}</a>"
                };
                configuration.BbCodes["amazon"] = new BasicBbCode
                {
                    Template = "<a href=\"http://tracker.pokefans.net/amazon/?suche={0}\">{0}</a>",
                    TemplateParameterized = "<a href=\"http://tracker.pokefans.net/amazon/?suche={1}\">{0}</a>"
                };
                configuration.BbCodes["size"] = new BasicBbCode
                {
                    Template = "{0}",
                    TemplateParameterized = "<span style=\"font-size: {1}%\">{0}</span>"
                };
                configuration.BbCodes["color"] = new BasicBbCode
                {
                    Template = "{0}",
                    TemplateParameterized = "<span style=\"color: {1}\">{0}</span>"
                };
                configuration.BbCodes["table"] = new BasicBbCode
                {
                    Template = "<div class=\"table\"><table>{0}</table></div>"
                };
                configuration.BbCodes["row"] = new BasicBbCode
                {
                    Template = "<tr>{0}</tr>",
                    TemplateParameterized = "<tr class=\"z{1}\">{0}</tr>",
                };
                configuration.BbCodes["cell"] = new BasicBbCode
                {
                    Template = "<tdr>{0}</td>",
                };

                return configuration;
            }
        }
    }
}
