// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Models
{
    public class CommentViewModel
    {
        public CommentViewModel(Comment comment)
        {
            this.Context = comment.Context;
            this.CommentedObjectId = comment.CommentedObjectId;
            this.ParentId = comment.ParentCommentId;
            this.CommentId = comment.Id;
            this.Author = comment.Author.UserName;
            this.AvatarFileName = comment.Author.MiniAvatarFileName;
            this.Text = comment.DisplayPublic ? comment.ParsedComment : "<p class=\"help-block\">Der Inhalt dieses Kommentars wurde von der Moderation versteckt.</p>";
            this.SubmitTime = comment.SubmitTime.ToString("d.m.Y H:i");
            this.Level = comment.Level;
        }

        public int Context { get; set; }
        public int CommentedObjectId { get; set; }
        public int? ParentId { get; set; }
        public int CommentId { get; set; }
        public string Author { get; set; }
        public string AvatarFileName { get; set; }
        public string Text { get; set; }
        public string SubmitTime { get; set; }
        public bool IsDeletable { get; set; }
        public int Level { get; set; }

        public IEnumerable<CommentViewModel> Children { get; set; }
    }
}