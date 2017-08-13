// Copyright 2016 the pokefans-core authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Comments;
using Pokefans.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pokefans.Caching;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Pokefans.Data.ViewModels;

namespace Pokefans.Util.Comments
{
    public enum CommentContext
    {
        Fanart = 1,
        Movesets = 2,
        Content = 3,
        Trading = 4
    }

    public abstract class CommentManager
    {
        protected Entities db;
        protected Cache cache;
        protected HttpContextBase httpContext;

        protected abstract CommentContext context { get; }

        public CommentManager(Entities ents, Cache c, HttpContextBase b)
        {
            db = ents;
            cache = c;
            httpContext = b;
        }

        public virtual bool CanDelete(User currentUser, IComment c)
        {
            if (currentUser.Id == c.AuthorId && !c.HasChildren && DateTime.Now - c.SubmitTime <= TimeSpan.FromDays(5))
                return true;
            if (currentUser.IsInRole("comment-moderator", cache, db))
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Comment LoadCommentWithChildrenById(int id, Entities sdb)
        {
            List<int> ids = sdb.Database.SqlQuery<int>(
            @"SELECT Comments.Id FROM CommentAncestors
             INNER JOIN Comments ON(Comments.Id = CommentAncestors.CommentId)
             JOIN system_users ON(system_users.Id = Comments.AuthorId)
             WHERE
                 CommentAncestors.AncestorId = @p0
             UNION
                 SELECT Comments.Id FROM CommentAncestors
                 INNER JOIN Comments ON(Comments.Id = CommentAncestors.AncestorId)
                 JOIN system_users ON(system_users.Id = AuthorId)
                 WHERE
                    Comments.Id = @p0
             UNION
                 SELECT Comments.Id FROM Comments
                 JOIN system_users ON(system_users.Id = Comments.AuthorId)
                 WHERE Comments.Id = @p0",
            new MySqlParameter("p0",id)).ToList();

            Dictionary<int,Comment> comments = sdb.Comments.Where(x => ids.Contains(x.Id))
                                                           .OrderBy(x => x.Level)
                                                           .OrderByDescending(x => x.SubmitTime)
                                                           .ToDictionary(x => x.Id);

            return buildCommentTree(comments, id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CommentViewModel> GetCommentsForObjectId(int id)
        {
            Dictionary<int, CommentViewModel> comments = db.Database.SqlQuery<CommentViewModel>(
                @"SELECT Comments.Id AS CommentId, Comments.SubmitTime, Comments.ParsedComment AS rawText, 
                         Comments.DisplayPublic, Comments.ParentCommentId, Comments.Level, Comments.Context,
                         Comments.CommentedObjectId, Comments.AuthorId, system_users.name AS Author, system_users.color, 
                         system_users.mini_avatar_filename AS AvatarFileName,
                         system_users.GravatarEnabled AS GravatarEnabled, system_users.email AS Email
                FROM CommentAncestors
                INNER JOIN Comments ON(Comments.Id = CommentId)
                JOIN system_users ON(system_users.Id = AuthorId)
                WHERE
                    AncestorId IS NULL
                AND
                    Context = @p2
                AND
                    CommentedObjectId = @p1
                ORDER BY
                    Level ASC,
                    SubmitTime DESC", new MySqlParameter("p1", id), new MySqlParameter("p2", Convert.ChangeType(context, context.GetTypeCode()))
                ).ToDictionary(x => x.CommentId);

            return buildCommentTree(comments);
        }

        private void validateComment(Comment comment)
        {
            if (comment.ParentCommentId.HasValue)
            {
                if (db.Comments.Any(x => x.Id == comment.ParentCommentId && x.CommentedObjectId != comment.CommentedObjectId))
                {
                    throw new CommentException("Parent commented object ID mismatch");
                }
                if (db.Comments.Any(x => x.Id == comment.ParentCommentId && x.Context != comment.Context))
                {
                    throw new CommentException("Parent context does not match this comments context");
                }
            }
        }

        /// <summary>
        /// Used when editing a comment
        /// </summary>
        /// <param name="comment"></param>
        public virtual void SaveComment(Comment comment)
        {
            validateComment(comment);
            db.SetModified(comment);
            db.SaveChanges();
        }

        /// <summary>
        /// used when adding a new comment into the database
        /// </summary>
        /// <param name="comment"></param>
        public virtual void AddComment(Comment comment)
        {
            validateComment(comment);

            db.Comments.Add(comment);
            db.SaveChanges();

            if (comment.ParentCommentId.HasValue)
            {
                List<int?> parents = db.CommentAncestors.Where(x => x.CommentId == comment.ParentCommentId.Value).Select(x => x.AncestorId).ToList();
                parents.Add(comment.ParentCommentId.Value);
                foreach (var parent in parents)
                {
                    db.CommentAncestors.Add(new CommentAncestor() { AncestorId = parent, CommentId = comment.Id });
                }
                comment.Level = (short)(db.Comments.Where(x => x.Id == comment.ParentCommentId.Value).Select(x => x.Level).First() + 1);
            }
            else
            {
                db.CommentAncestors.Add(new CommentAncestor() { AncestorId = null, CommentId = comment.Id });
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes a comment and all its descendants.
        /// </summary>
        /// <param name="comment">The Comment to delete</param>
        public virtual void DeleteComment(Comment comment)
        {
            db.Comments.Remove(comment);
            db.CommentAncestors.Remove(db.CommentAncestors.First(x => x.CommentId == comment.Id)); //TODO: Optimize?

            foreach (Comment cmt in comment.Children)
            {
                DeleteComment(cmt);
            }
        }

        /// <summary>
        /// Builds the Comment tree of a given dictionary of the form [int : commentId => Comment : comment], and returns a List of the root nodes of the tree.
        /// </summary>
        /// <param name="results">Database results in form [int : commentId => Comment : comment]</param>
        /// <returns>List of root nodes with children populated</returns>
        protected List<CommentViewModel> buildCommentTree(Dictionary<int, CommentViewModel> results)
        {
            List<CommentViewModel> rootComments = new List<CommentViewModel>();
            
            foreach(KeyValuePair<int, CommentViewModel> c in results)
            {
                if(c.Value.Level == 0)
                {
                    rootComments.Add(c.Value);
                }
                else
                {
                    // just for safety reasons. Maybe we should log this?
                    if (c.Value.ParentCommentId.HasValue)
                    {
                        results[c.Value.ParentCommentId.Value].Children.Add(c.Value);
                    }
                }
            }

            return rootComments;
        }

        /// <summary>
        /// Builds the comment tree, and returns a single Comment with it's child comments populated.
        /// </summary>
        /// <param name="results">Dictionary with the Comments as the results and their IDs as Keys</param>
        /// <param name="targetId">CommentId of the target Comment</param>
        /// <returns>Comment of the specified id, with the children populated.</returns>
        protected static Comment buildCommentTree(Dictionary<int, Comment> results, int targetId)
        {
            Comment rootComment = null;

            foreach (KeyValuePair<int, Comment> c in results)
            {
                if (c.Key == targetId)
                {
                    rootComment = c.Value;
                    continue; // we're not interested in parents of this comment.
                }
                if (c.Value.Level != 0)
                {
                    // just for safety reasons. Maybe we should log this?
                    if (c.Value.ParentCommentId.HasValue)
                    {
                        results[c.Value.ParentCommentId.Value].Children.Add(c.Value);
                    }
                }

            }
            if(rootComment == null)
            {
                throw new ArgumentException("TargetId not in results");
            }

            return rootComment;
        }
    }
}
