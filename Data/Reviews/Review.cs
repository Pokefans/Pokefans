// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Reviews
{
    public class Review
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public byte Rating { get; set; }

        public string Text { get; set; }

        public string TextCode { get; set; }

        public string CreatorIp { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int ReviewItemId { get; set; }

        [ForeignKey("ReviewItemId")]
        public ReviewItem Item { get; set; }
    }
}

