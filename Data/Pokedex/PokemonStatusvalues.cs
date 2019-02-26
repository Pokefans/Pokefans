// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    public class PokemonStatusvalues
    {
        public int HP { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int SpecialAttack { get; set; }

        public int SpecialDefense { get; set; }

        public int Speed { get; set; }

        public bool Any()
        {
            return HP != 0 || Attack != 0 || Defense != 0 || SpecialAttack != 0 || SpecialDefense != 0 || Speed != 0;
        }
    }
}
