// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Security;

namespace Pokefans.Util.Comments
{
    public class MovesetCommentManager : CommentManager
    {
        public MovesetCommentManager(Entities ents, Cache c, HttpContextBase b) : base(ents, c, b)
        {
        }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Movesets;
            }
        }

        // TODO: Add, Delete
    }
}
