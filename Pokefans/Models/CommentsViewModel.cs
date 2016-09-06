// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Comments;
using Pokefans.Data.ViewModels;
using Pokefans.Util.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Models
{
    public class CommentsViewModel
    {
        public int CommentedObjectId { get; set; }

        public CommentContext Context { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public bool CanHideComment { get; set; }

        public CommentManager Manager { get; set; }

        public User CurrentUser { get; set; }

        public int Level { get; set; }

        public CommentsViewModel Descend(List<CommentViewModel> comments)
        {
            return new CommentsViewModel()
            {
                CanHideComment = CanHideComment,
                CommentedObjectId = CommentedObjectId,
                Context = Context,
                Comments = comments,
                CurrentUser = CurrentUser,
                Level = Level + 1,
                Manager = Manager
            };
        }
    }
}