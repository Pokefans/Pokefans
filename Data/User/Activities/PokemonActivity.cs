using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.UserData.Activities
{
    public class PokemonActivity : UserActivity
    {
        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }
    }
}