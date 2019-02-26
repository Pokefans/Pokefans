// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;
using Pokefans.Data.FriendCodes;

namespace Pokefans.Data.Service
{
    public class FriendSafari
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

        public int PokemonTypeId { get; set; }

        [ForeignKey("PokemonTypeId")]
        public PokemonType Type { get; set; }

        public int Pokemon1Id { get; set; }

        [ForeignKey("Pokemon1Id")]
        public Pokemon Pokemon1 { get; set; }

        public int Pokemon2Id { get; set; }

        [ForeignKey("Pokemon2Id")]
        public Pokemon Pokemon2 { get; set; }

        public int Pokemon3Id { get; set; }

        [ForeignKey("Pokemon3Id")]
        public Pokemon Pokemon3 { get; set; }

        public string Annotations { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
