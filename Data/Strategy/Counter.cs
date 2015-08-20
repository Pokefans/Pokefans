// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.Strategy
{
    public class Counter
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int CounterId { get; set; }

        [ForeignKey("CounterId")]
        public Pokemon CounterPokemon { get; set; }

        public PokemonGeneration Generation;

        [MaxLength(1024)]
        public string Remarks { get; set; }

        [MaxLength(768)]
        public string RemarksCode { get; set; }
    }
}

