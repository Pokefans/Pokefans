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

namespace Pokefans.Util.Comments
{
    public enum CommentContext
    {
        Fanart = 1,
        Movesets = 2,
        Content = 3
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment LoadCommentWithoutChildren(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery(
                "SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM CommentAncestors" +
                "INNER JOIN Comments ON(Comments.Id = CommentAncestors.AncestorId)" +
                "JOIN system_users ON(system_users.Id = Comments.AuthorId)" +
                "WHERE" +
                "    CommentAncestors.CommentId = @p0" +
                "UNION" +
                "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM Comments" +
                "    JOIN system_users ON(system_users.Id = AuthorId)" +
                "    WHERE id = @p0" +
                "ORDER BY" +
                "    Level ASC,"+
                "    SubmitTime DESC",
            id).ToDictionary(x => x.Id);

            return buildCommentTree(comments, id);
        }

        public virtual bool CanDelete(User currentUser, Comment c)
        {
            if (currentUser.Id == c.AuthorId && c.Children.Count == 0 && DateTime.Now - c.SubmitTime <= TimeSpan.FromDays(5))
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
            Dictionary<int, Comment> comments = sdb.Comments.SqlQuery("SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM CommentAncestors" +
            "INNER JOIN Comments ON(Comments.Id = CommentAncestors.CommentId)" +
            "JOIN system_users ON(system_users.Id = Comments.AuthorId)" +
            "WHERE" +
            "    CommentAncestors.AncestorId = @p0" +
            "UNION" +
            "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM CommentAncestors" +
            "    INNER JOIN Comments ON(Comments.Id = CommentAncestors.AncestorId)" +
            "    JOIN system_users ON(system_users.Id = AuthorId)" +
            "    WHERE" +
            "        Id = @p0" +
            "UNION" +
            "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM Comments" +
            "    JOIN system_users ON(system_users.Id = Comments.AuthorId)" +
            "    WHERE Id = @p0" +
            "ORDER BY" +
            "    Level ASC," +
            "    SubmitTime DESC",
            id).ToDictionary(x => x.Id);

            return buildCommentTree(comments, id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Comment> LoadAll(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery(
                @"SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM CommentAncestors
                INNER JOIN Comments ON(Comments.Id = CommentAncestors.CommentId)
                JOIN system_users ON(system_users.Id = Comments.AuthorId)
                WHERE
                    ancestor = 0
                AND
                    context = @p1
                AND
                    commented_object_id = @p0
                ORDER BY
                    Level ASC,
                    SubmitTime DESC", id, (int)context).ToDictionary(x => x.Id);

            return buildCommentTree(comments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Comment> GetCommentsForObjectId(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery(
                @"SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_filename FROM CommentAncestors
                INNER JOIN Comments ON(Comments.Id = CommentId)
                JOIN system_users ON(system_users.Id = AuthorId)
                WHERE
                    AncestorId = 0
                AND
                    Context = @p2
                AND
                    CommentedObjectId = @p1
                ORDER BY
                    Level ASC,
                    SubmitTime DESC", new MySqlParameter("p1", id), new MySqlParameter("p2", Convert.ChangeType(context, context.GetTypeCode()))
                ).ToDictionary(x => x.Id);

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
                List<int> parents = db.CommentAncestors.Where(x => x.CommentId == comment.ParentCommentId.Value).Select(x => x.AncestorId).ToList();
                parents.Add(comment.ParentCommentId.Value);
                foreach (var parent in parents)
                {
                    db.CommentAncestors.Add(new CommentAncestor() { AncestorId = parent, CommentId = comment.Id });
                }
                comment.Level = (short)(db.Comments.Where(x => x.Id == comment.ParentCommentId.Value).Select(x => x.Level).First() + 1);
            }
            else
            {
                db.CommentAncestors.Add(new CommentAncestor() { AncestorId = 0, CommentId = comment.Id });
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

            foreach (var cmt in comment.Children)
            {
                DeleteComment(cmt);
            }
        }

        /// <summary>
        /// Builds the Comment tree of a given dictionary of the form [int : commentId => Comment : comment], and returns a List of the root nodes of the tree.
        /// </summary>
        /// <param name="results">Database results in form [int : commentId => Comment : comment]</param>
        /// <returns>List of root nodes with children populated</returns>
        protected List<Comment> buildCommentTree(Dictionary<int, Comment> results)
        {
            List<Comment> rootComments = new List<Comment>();

            foreach(KeyValuePair<int, Comment> c in results)
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
                if (c.Value.Level != 0)
                { 
                    results[c.Key].Children.Add(c.Value);
                }
                if(c.Key == targetId)
                {
                    rootComment = c.Value;
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
