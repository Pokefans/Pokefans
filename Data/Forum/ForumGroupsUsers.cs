using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class ForumGroupsUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public bool IsLeader { get; set; }

        [ForeignKey("GroupId")]
        public ForumGroup Group { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
