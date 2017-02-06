// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.Wifi
{
    public class TradeLog
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int Price { get; set; }

        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

        public int UserToId { get; set; }

        [ForeignKey("UserToId")]
        public User UserTo { get; set; }

        public int UserFromId { get; set; }

        [ForeignKey("UserFromId")]
        public User UserFrom { get; set; }

        public DateTime CompletedTime { get; set; }

        public DateTime ValidOn { get; set; }
    }
}

