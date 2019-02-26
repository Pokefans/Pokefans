using System;
using Pokefans.Data.Strategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.UserData.Activities
{
    public class MovesetActivity : PokemonActivity
    {
        public int MovesetId { get; set; }

        [ForeignKey("MovesetId")]
        public Moveset Moveset { get; set; }
    }

    public class MovesetInsertActivity : MovesetActivity
    {

    }

    public class MovesetUpdateActivity : MovesetActivity
    {

    }

    public class MovesetApproveActiviy : MovesetActivity
    {

    }
}

