// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Pokefans.Data.Pokedex
{
    public class PokemonEditionGroup
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Markup { get; set; }

        private ICollection<PokemonEdition> pokemonEditions;

        [InverseProperty("EditionGroup")]
        public virtual ICollection<PokemonEdition> PokemonEditions
        {
            get { return pokemonEditions ?? (pokemonEditions = new HashSet<PokemonEdition>()); }
            set { pokemonEditions = value; }
        }
    }
}

