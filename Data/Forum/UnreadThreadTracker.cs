using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class UnreadThreadTracker
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ThreadId { get; set; }

        public DateTime ResetTime { get; set; }
    }
}
