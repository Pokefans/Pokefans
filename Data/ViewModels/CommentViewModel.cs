using Pokefans.Data.Comments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.ViewModels
{
    public class CommentViewModel : IComment
    {
        public CommentViewModel()
        {
            Children = new List<CommentViewModel>();
        }

        public CommentViewModel(Comment c)
        {
            Context = c.Context;
            CommentedObjectId = c.CommentedObjectId;
            ParentCommentId = c.ParentCommentId;
            CommentId = c.Id;
            Author = c.Author.UserName;
            AuthorId = c.AuthorId;
            AvatarFileName = c.Author.MiniAvatarFileName;
            DisplayPublic = c.DisplayPublic;
            Text = c.ParsedComment;
            SubmitTime = c.SubmitTime;
            Level = c.Level;

            Children = new List<CommentViewModel>();
            foreach (var child in c.Children)
            {
                Children.Add(new CommentViewModel(child));
            }
        }

        public int Context { get; set; }
        public int CommentedObjectId { get; set; }
        public int? ParentCommentId { get; set; }
        public int CommentId { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public string AvatarFileName { get; set; }
        public bool DisplayPublic { get; set; }

        private string rawText { get; set; }
        public string Text
        {
            get { return DisplayPublic ? rawText : "<p class=\"help-block\">Der Inhalt dieses Kommentars wurde von der Moderation versteckt.</p>"; }
            set { rawText = value; }
        }
        public DateTime SubmitTime { get; set; }
        public string SubmitTimeString
        {
            get
            {
                return SubmitTime.ToString("d.m.Y H:i");
            }
        }
        public bool IsDeletable { get; set; }
        public short Level { get; set; }

        public List<CommentViewModel> Children { get; set; }

        public bool HasChildren
        {
            get { return Children.Count > 0; }
        }
    }
}
