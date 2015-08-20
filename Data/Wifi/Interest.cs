// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Wifi
{
    public class Interest
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

        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }
    }
}