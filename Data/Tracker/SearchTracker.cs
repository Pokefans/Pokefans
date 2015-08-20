// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Tracker
{
    public class SearchTracker
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime Timestamp { get; set; }

        [MaxLength(39)]
        public string Ip { get; set; }

        [MaxLength(255)]
        public string Input { get; set; }

        [MaxLength(255)]
        public string InputUri { get; set; }

        [MaxLength(10)]
        public string Index { get; set; }

        [MaxLength(100)]
        public string Source { get; set; }

        [MaxLength(100)]
        public string Visitor { get; set; }
    }
}
