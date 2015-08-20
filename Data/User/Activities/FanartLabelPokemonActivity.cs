using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pokefans.Data.Fanwork;

namespace Pokefans.Data.UserData.Activities
{
    public class FanartLabelPokemonActivity : UserActivity
    {
        public int FanartId { get; set; }

        [ForeignKey("FanartId")]
        public Fanart Fanart { get; set; }
    }
}

