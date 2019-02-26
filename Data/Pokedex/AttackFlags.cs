// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;

namespace Pokefans.Data.Pokedex
{
    public class AttackFlags
    {
        public bool Contact { get; set; }

        public bool Bite { get; set; }

        public bool Punch { get; set; }

        public bool Sound { get; set; }

        public bool Puls { get; set; }

        public bool Recoil { get; set; }

        public short RecoilAmount { get; set; }

        public bool SelfHeal { get; set; }

        public bool HealIsAbsorbing { get; set; }

        public short HealAmount { get; set; }

        public bool ChangeDamage { get; set; }
    }
}

