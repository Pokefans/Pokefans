// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pokefans.Data.Comments;
using Pokefans.Data.Fanwork;
using Pokefans.Data.ViewModels;

namespace Pokefans.Util.Comments
{
    public class TradingCommentManager : CommentManager
    {
        public TradingCommentManager(Entities ents, Cache c, HttpContextBase b) : base(ents, c, b) { }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Trading;
            }
        }

        /// <summary>
        /// Adds a new Trading Comment to the database
        /// </summary>
        /// <param name="comment"></param>
        public override void AddComment(Comment comment)
        {
            if(comment.Context != (int)this.context)
            {
                throw new CommentException("This comment must be of type trading");
            }
            base.AddComment(comment);

            Offer o = db.WifiOffers.Single(x => x.Id == comment.CommentedObjectId);
            if(o == null)
            {
                throw new CommentException("Commented Object does not exist");
            }

            // TODO: Benachrichtigung senden
        }

        /// <summary>
        /// Additionally, within the fanart scope, Users can delete comments on their own works
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool CanDelete(User currentUser, IComment c)
        {
            if (currentUser == null)
                return false;

            Offer f = db.WifiOffers.Single(x => x.Id == c.CommentedObjectId);

            if (currentUser.Id == f.UserId)
                return true;

            return base.CanDelete(currentUser, c);
        }
    }
}
