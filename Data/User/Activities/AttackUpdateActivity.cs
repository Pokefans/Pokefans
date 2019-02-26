using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pokefans.Data.Pokedex;

namespace Pokefans.Data.UserData.Activities
{
    public class AttackUpdateActivity : UserActivity
    {
        public int AttackId { get; set; }

        [ForeignKey("AttackId")]
        public Attack Attack { get; set; }
    }
}

