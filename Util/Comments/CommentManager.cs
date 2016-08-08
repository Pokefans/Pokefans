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
        protected ApplicationUserManager userManager;
        protected Cache cache;
        protected HttpContextBase httpContext;
        private bool canSeeHiddenComments;

        protected abstract CommentContext context { get; }

        public CommentManager(Entities ents, ApplicationUserManager umgr, Cache c, HttpContextBase b)
        {
            db = ents;
            userManager = umgr;
            cache = c;
            httpContext = b;
            User currentUser = userManager.FindByNameAsync(httpContext.User.Identity.Name).Result;

            canSeeHiddenComments = currentUser.IsInRole("comment-moderator", cache, db);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment LoadCommentWithoutChildren(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery(
                "SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM CommentAncestors" +
                "INNER JOIN Comments ON(Comments.Id = CommentAncestors.AncestorId)" +
                "JOIN system_users ON(system_users.Id = Comments.AuthorId)" +
                "WHERE" +
                "    CommentAncestors.CommentId = @p0" +
                "UNION" +
                "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM Comments" +
                "    JOIN system_users ON(system_users.Id = AuthorId)" +
                "    WHERE id = @p0" +
                "ORDER BY" +
                "    Level ASC,"+
                "    SubmitTime DESC",
            id).ToDictionary(x => x.Id);

            return buildCommentTree(comments, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment LoadCommentWithChildrenById(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery("SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM CommentAncestors" +
            "INNER JOIN Comments ON(Comments.Id = CommentAncestors.CommentId)" +
            "JOIN system_users ON(system_users.Id = Comments.AuthorId)" +
            "WHERE" +
            "    CommentAncestors.AncestorId = @p0" +
            "UNION" +
            "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM CommentAncestors" +
            "    INNER JOIN Comments ON(Comments.Id = CommentAncestors.AncestorId)" +
            "    JOIN system_users ON(system_users.Id = AuthorId)" +
            "    WHERE" +
            "        Id = @p0" +
            "UNION" +
            "    SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM Comments" +
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
                @"SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM CommentAncestors
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
        public List<Comment> getCommentsForObjectId(int id)
        {
            Dictionary<int, Comment> comments = db.Comments.SqlQuery(
                "SELECT Comments.*, system_users.name, system_users.color, system_users.mini_avatar_file_name FROM CommentAncestors" +
                "INNER JOIN Comments ON(Comments.Id = CommentId)" +
                "JOIN system_users ON(system_users.Id = AuthorId)" +
                "WHERE" +
                "    AncestorId = 0" +
                "AND" +
                "    Context = @p2" +
                "AND" + 
                "    CommentedObjectId = @p1" +
                "ORDER BY" +
                "    Level ASC," +
                "    SubmitTime DESC",
                id, this.context).ToDictionary(x => x.Id);

            return buildCommentTree(comments);
        }

        public void SaveComment(Comment comment)
        {
            if(comment.ParentCommentId != null)
            {
                if(comment.ParentComment.CommentedObjectId != comment.CommentedObjectId)
                {
                    throw new CommentException("Parent commented object ID mismatch");
                }
                if (comment.Context != comment.ParentComment.Context)
                {
                    throw new CommentException("Parent context does not match this comments context");
                }
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
                    if (c.Value.DisplayPublic || canSeeHiddenComments)
                    {
                        results[c.Key].Children.Add(c.Value);
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
        protected Comment buildCommentTree(Dictionary<int, Comment> results, int targetId)
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
