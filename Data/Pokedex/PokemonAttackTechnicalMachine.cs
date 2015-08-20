// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Pokedex
{
    public class PokemonAttackTechnicalMachine : PokemonAttack
    {
        public int AttackTechnicalMachineId { get; set; }

        [ForeignKey("AttackTechnicalMachineId")]
        public AttackTechnicalMachine AttackTechnicalMachine { get; set; }
    }
}

