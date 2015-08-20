// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    [Table("dex_pokemon_tiers")]
    public class PokemonTiers
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        public int TierId { get; set; }

        public PokemonGeneration Generation { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        [ForeignKey("TierId")]
        public PokemonTier Tier { get; set; }
    }
}
