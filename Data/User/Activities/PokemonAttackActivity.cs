using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.UserData.Activities
{
    public enum UpdateMode
    {
        Create,
        Update,
        Delete}

    ;

    public class PokemonAttackActivity : PokemonActivity
    {
        public int AttackId { get; set; }

        [ForeignKey("AttackId")]
        public Attack Attack { get; set; }

        public UpdateMode Mode;
    }
}