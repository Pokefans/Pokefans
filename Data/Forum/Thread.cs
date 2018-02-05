using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    public class Thread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int AuthorId { get; set; }

        [MaxLength(255)]
        [Index]
        public string Title { get; set; }

        public DateTime ThreadStartTime { get; set; }

        public int ThreadIcon { get; set; }

        public int Replies { get; set; }

        public int Visits { get; set; }

        public int BoardId { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }
    }
}
