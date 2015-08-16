using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    public class PokemonAbilities
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the pokemon identifier.
        /// </summary>
        /// <value>
        /// The pokemon identifier.
        /// </value>
        public int PokemonId { get; set; }

        /// <summary>
        /// Gets or sets the ability identifier.
        /// </summary>
        /// <value>
        /// The ability identifier.
        /// </value>
        public int AbilityId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is mega.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is mega; otherwise, <c>false</c>.
        /// </value>
        public bool IsMega { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dreamworld.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dreamworld; otherwise, <c>false</c>.
        /// </value>
        public bool IsDreamworld { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        [ForeignKey("AbilityId")]
        public Ability Ability { get; set; }
    }
}
