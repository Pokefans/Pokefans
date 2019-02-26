// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Pokedex
{
    public class PokemonAttackHiddenMachine : AttackHiddenMachine
    {
        public int AttackHiddenMachineId { get; set; }

        [ForeignKey("AttackHiddenMachineId")]
        public AttackHiddenMachine AttackHiddenMachine { get; set; }
    }
}

