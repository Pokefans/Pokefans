// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex.Ranger
{
    public class RangerPokemon
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public PokemonType Group { get; set; }

        public int PokePowerId { get; set; }

        [ForeignKey("PokePowerId")]
        public PokePower PokePower { get; set; }

        public int RangerAbilityId { get; set; }

        [ForeignKey("RangerAbilityId")]
        public RangerAbility Ability { get; set; }

        public int AbilityPower { get; set; }

        public int Circles { get; set; }

        public int RangerVersion { get; set; }

        public int EscapeValue { get; set; }

        public int Experience { get; set; }

        public string CatchHint { get; set; }

        public string CatchHintCode { get; set; }
    }
}