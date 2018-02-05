using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int? AuthorId { get; set; }

        public int ThreadId { get; set; }

        public int BoardId { get; set; }

        [MaxLength(255)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public string BodyRaw { get; set; }

        public DateTime PostTime { get; set; }

        public DateTime LastEditTime { get; set; }

        public string EditReason { get; set; }

        public bool IsSolution { get; set; }

        [MaxLength(48)]
        public string IpAddress { get; set; }

        public bool NeedsApproval { get; set; }

        public int ReactionHeart { get; set; }

        public int ReactionLol { get; set; }

        public int ReactionThumbsUp { get; set; }

        [ForeignKey("ThreadId")]
        public Thread Thread { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }
    }
}
