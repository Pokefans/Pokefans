using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class UnreadForumTracker
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public DateTime ResetTime { get; set; }
    }
}
