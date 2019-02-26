// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class PokemonTypeEffectivity
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int Type1Id { get; set; }

        [ForeignKey("Type1Id")]
        public PokemonType Type1 { get; set; }

        public int Type2Id { get; set; }

        [ForeignKey("Type2Id")]
        public PokemonType Type2 { get; set; }

        public decimal Relation { get; set; }
    }
}

