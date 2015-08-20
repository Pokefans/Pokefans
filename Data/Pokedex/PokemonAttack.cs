// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class PokemonAttack
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int EditionId { get; set; }

        [ForeignKey("EditionId")]
        public PokemonEdition Edition { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int AttackId { get; set; }

        [ForeignKey("AttackId")]
        public Attack Attack { get; set; }

        public string Details { get; set; }

        public string DetailsCode { get; set; }
    }
}

