// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Tracker
{
    public class UserAdminTracker
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int ModeratorId { get; set; }

        [ForeignKey("ModeratorId")]
        public User Moderator { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [MaxLength(20)]
        public string Modul { get; set; }

        public DateTime Timestamp { get; set; }
    }
}