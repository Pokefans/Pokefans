// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Data.Comments;
using Pokefans.Security;

namespace Pokefans.Util.Comments
{
    public class ContentCommentManager : CommentManager
    {
        public ContentCommentManager(Entities ents, Cache c, HttpContextBase b) : base(ents, c, b)
        {
        }

        protected override CommentContext context
        {
            get
            {
                return CommentContext.Content;
            }
        }

        /// <summary>
        /// Overrides the default AddComment to add some safety guards.
        /// </summary>
        /// <param name="comment"></param>
        public override void AddComment(Comment comment)
        {
            Content c = db.Contents.Single(x => x.Id == comment.Id);
            if(!c.CommentsEnabled)
            {
                throw new CommentException("Commenting is not enabled on this Content");
            }
            base.AddComment(comment);

            c.CommentCount++;
            db.SetModified(c);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes the comment and decrements the content comment count.
        /// </summary>
        /// <param name="comment"></param>
        public override void DeleteComment(Comment comment)
        {
            base.DeleteComment(comment);

            Content c = db.Contents.Find(comment.CommentedObjectId);
            c.CommentCount--;
            db.SetModified(c);
            db.SaveChanges();
        }
    }
}
