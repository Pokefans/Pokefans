// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.FriendCodes;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.Service
{
    public class QrCode
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int FriendCodeId { get; set; }

        [ForeignKey("FriendCodeId")]
        public Friendcode Friendcode { get; set; }

        public string IngameName { get; set; }

        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public PokemonLocation Location { get; set; }

        public string Slot { get; set; }

        public DateTime LastUpate { get; set; }

        public string ImageName { get; set; }

        public string OriginalImageName { get; set; }
    }
}