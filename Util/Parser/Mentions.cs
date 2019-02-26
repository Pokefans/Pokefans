// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Configuration;
using Pokefans.Data;
using System.Linq;

namespace Pokefans.Util.Parser
{
    struct MentionCandidate 
    {
        public string User { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// The Mentions Parser is kept intentionally different from the rest of the
    /// parser infrastructure, as the automatas ultimately differ to much, and
    /// this class has a public state, while the other does not.
    /// Also, we need to parse all mentioned users into a list.
    /// </summary>
    public class Mentions
    {
        private Entities db;

        public Mentions(Entities ents, string content)
        {
            Users = new List<string>();
            db = ents;
            Parsed = Parse(content);
        }

        /// <summary>
        /// Contains the (unique) User-Urls that correspond to the mentioned
        /// users.
        /// </summary>
        /// <value>The users.</value>
        public List<string> Users { get; set; }

        public string Parsed { get; set; }

        enum ParserStatus { Out, User };

        ParserStatus s = ParserStatus.Out;

        private string Parse(string contents)
        {
            string final = "";
            string current = "";
            MentionCandidate candidate = new MentionCandidate();
            int count = 0;

            foreach(char c in contents) {

                if(s == ParserStatus.Out) {
                    if (c == '@')
                    {
                        s = ParserStatus.User;
                        final += current;
                        current = "";
                    }
                    else
                    {
                        current += c;
                    }
                }
                else if (s == ParserStatus.User) {
                    if(c == ' ' || c == ',')
                    {
                        string url = db.Users.Where(x => x.UserName == current).Select(x => x.Url).FirstOrDefault();
                        if(url != null) {
                            candidate.User = current;
                            candidate.Url = url;
                        }
                    }
                    current += c;
                    count++;
                    // maximum username length is 43 characters
                    if(count > 43 || c == '@') {
                        final += "<a href=\"https://user." + ConfigurationManager.AppSettings["Domain"] + "/profil/" + candidate.Url + "\">@"+candidate.User+"</a>";
                        Users.Add(candidate.Url);
                        final += current.Replace(candidate.User, "");
                        count = 0;

                        if (c == '@') s = ParserStatus.User;
                        else s = ParserStatus.Out;
                        current = "";
                    }
                }
            }
            return final;
        }
    }
}
