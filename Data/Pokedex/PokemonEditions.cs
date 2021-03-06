﻿// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    public class PokemonEdition
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public PokemonGeneration Generation { get; set; }

        public string Name { get; set; }

        public int PokemonEditionGroupId { get; set; }

        [ForeignKey("PokemonEditionGroupId")]
        public PokemonEditionGroup EditionGroup { get; set; }
    }
}
