// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;

namespace Pokefans.Util.Parser
{
    public abstract class InsideCode
    {
        public abstract string Format(InsideCodeToken token);
    }

    public class LinkInsideCode : InsideCode
    {
        public override string Format(InsideCodeToken token)
        {
            if (token.Parameters == null || token.Parameters.Length == 0)
            {
                return token.Contents;
            }

            var url = token.Parameters[0];
            var text = token.Parameters.Length > 1 ? token.Parameters[1] : url;

            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("/"))
            {
                url = "http://" + url;
            }

            return String.Format("<a href=\"{0}\">{1}</a>", url, text);
        }
    }

    public class ImgInsideCode : InsideCode
    {
        public override string Format(InsideCodeToken token)
        {
            if (token.Parameters == null || token.Parameters.Length == 0)
            {
                return token.Contents;
            }

            var url = token.Parameters[0];
            var text = token.Parameters.Length > 1 ? token.Parameters[1] : string.Empty;
            var @class = token.Parameters.Length > 2 ? token.Parameters[2] : string.Empty;

            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("/"))
            {
                url = "http://" + url;
            }

            return String.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" class=\"{2}\" />", url, text, @class);
        }
    }
}
