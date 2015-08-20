// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Pokefans.Data.Pokedex
{
    public class AttackTargetType
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        private ICollection<AttackTarget> attackTargets;

        [InverseProperty("TargetType")]
        public virtual ICollection<AttackTarget> AttackTargets
        {
            get { return attackTargets ?? (attackTargets = new HashSet<AttackTarget>()); }
            set { attackTargets = value; }
        }
    }
}

