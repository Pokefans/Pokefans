using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public enum ThreadType { Normal = 0, Sticky = 1, Announcement = 2, GlobalAnnouncement = 3 }

    public class Thread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? AuthorId { get; set; }

        [MaxLength(255)]
        [Index]
        public string Title { get; set; }

        public DateTime ThreadStartTime { get; set; }

        public int ThreadIcon { get; set; }

        public int Replies { get; set; }

        public int Visits { get; set; }

        public int BoardId { get; set; }

        public ThreadType Type { get; set; }

        public bool IsLocked { get; set; }

        public int LastPostId { get; set; }

        public int? PrefixId { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

    }
}
