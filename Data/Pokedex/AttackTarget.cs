// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class AttackTarget
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int AttackId { get; set; }

        public int AttackTargetTypeId { get; set; }

        public PokemonGeneration Generation { get; set; }

        [ForeignKey("AttackId")]
        public virtual Attack Attack { get; set; }

        [ForeignKey("AttackTargetTypeId")]
        public virtual AttackTargetType TargetType { get; set; }
    }
}

