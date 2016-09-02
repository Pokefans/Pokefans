// Copyright 2016 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Comments
{
    public class CommentAncestor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Index]
        public int CommentId { get; set; }

        [Index]
        public int AncestorId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }

        [ForeignKey("AncestorId")]
        public Comment Ancestor { get; set; }
    }
}
