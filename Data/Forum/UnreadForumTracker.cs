using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class UnreadForumTracker
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public DateTime ResetTime { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
