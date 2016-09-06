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
    public class FanartCommentManager : CommentManager
    {
        public FanartCommentManager(Entities ents, Cache c, HttpContextBase b) : base(ents, c, b) { }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Fanart;
            }
        }

        /// <summary>
        /// Adds a new Fanart Comment to the database, incrementing the comment counter.
        /// </summary>
        /// <param name="comment"></param>
        public override void AddComment(Comment comment)
        {
            if(comment.Context != (int)CommentContext.Fanart)
            {
                throw new CommentException("This comment must be of type fanart");
            }
            base.AddComment(comment);

            Fanart f = db.Fanarts.Single(x => x.Id == comment.CommentedObjectId);
            if(f == null)
            {
                throw new CommentException("Commented Object does not exist");
            }
            f.CommentCount++;
            db.SetModified(f);
            db.SaveChanges();

            // TODO: Benachrichtigung senden
        }

        /// <summary>
        /// Override default action to reduce comment counter.
        /// </summary>
        /// <param name="comment"></param>
        public override void DeleteComment(Comment comment)
        {
            base.DeleteComment(comment);
            Fanart f = db.Fanarts.Single(x => x.Id == comment.CommentedObjectId);
            f.CommentCount--;
            db.SetModified(f);
            db.SaveChanges();
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

            Fanart f = db.Fanarts.Single(x => x.Id == c.CommentedObjectId);

            if (currentUser.Id == f.UploadUserId)
                return true;

            return base.CanDelete(currentUser, c);
        }
    }
}
