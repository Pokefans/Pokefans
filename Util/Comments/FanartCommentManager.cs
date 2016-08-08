using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pokefans.Util.Comments
{
    class FanartCommentManager : CommentManager
    {
        public FanartCommentManager(Entities ents, ApplicationUserManager umgr, Cache c, HttpContextBase b) : base(ents, umgr, c, b) { }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Fanart;
            }
        }
    }
}
