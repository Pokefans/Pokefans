// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex.Ranger
{
    public class RangerPokemonLocation
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int RangerPokemonId { get; set; }

        public int RangerVersion { get; set; }

        public bool IsStoryLocation { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }
    }
}