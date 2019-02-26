// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Models
{
    public class NewCommentViewModel
    {
        public string Text { get; set; }
        public int Context { get; set; }
        public int CommentedObjectId { get; set; }
        public int? ParentId { get; set; }
    }
}