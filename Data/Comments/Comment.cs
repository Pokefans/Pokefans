// Copyright 2016 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Comments
{
    public class Comment : IComment
    {
        public Comment()
        {
            this.Children = new List<Comment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public DateTime SubmitTime { get; set; }

        public DateTime EditTime { get; set; }

        [MaxLength(8096)]
        public string UnparsedComment { get; set; }

        [MaxLength(8096)]
        public string ParsedComment { get; set; }

        public bool DisplayPublic { get; set; }

        public int CommentedObjectId { get; set; }

        public int? ParentCommentId { get; set; }

        public short Level { get; set; }

        public int Context { get; set; }

        [NotMapped]
        public bool HasChildren
        {
            get { return Children.Count > 0; }
        }

        [NotMapped]
        public List<Comment> Children { get; set; }

        [ForeignKey("ParentCommentId")]
        public Comment ParentComment { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

    }
}
