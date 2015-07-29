using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pokefans.Data.Pokedex
{
    public enum EvolutionMethod { Item, Trade, Level, Other };

    public class PokemonEvolutionMethod
    {
        public EvolutionMethod Method { get; set; }

        public int? ItemId { get; set; }

        public int Level { get; set; }

        public string Description { get; set; }
    }
}
