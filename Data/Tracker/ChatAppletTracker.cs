// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Tracker
{
    public class ChatAppletTracker
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
        public string Host { get; set; }

        [MaxLength(30)]
        public string Nickname { get; set; }

        [MaxLength(10)]
        public string AppletName { get; set; }
    }
}

