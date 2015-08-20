// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class PokemonItem
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int EditionId { get; set; }

        [ForeignKey("EditionId")]
        public PokemonEdition Edition { get; set; }

        public int EditionGroupId { get; set; }

        [ForeignKey("EditionGroupId")]
        public PokemonEditionGroup EditionGroup { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [MaxLength(255)]
        public string Hint { get; set; }
    }
}

