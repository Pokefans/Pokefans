using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    /// <summary>
    /// 
    /// </summary>
    public class PokemonId
    {
        /// <summary>
        /// Gets or sets the national id.
        /// </summary>
        /// <value>
        /// The national id.
        /// </value>
        public int National { get; set; }

        /// <summary>
        /// Gets or sets the kanto id (1st generation)
        /// </summary>
        /// <value>
        /// The kanto id.
        /// </value>
        public int? Kanto { get; set; }

        /// <summary>
        /// Gets or sets the johto id (2nd generation)
        /// </summary>
        /// <value>
        /// The johto id.
        /// </value>
        public int? Johto { get; set; }

        /// <summary>
        /// Gets or sets the hoenn id (3rd generation)
        /// </summary>
        /// <value>
        /// The hoenn id.
        /// </value>
        public int? Hoenn { get; set; }

        /// <summary>
        /// Gets or sets the sinnoh id (4th generation)
        /// </summary>
        /// <value>
        /// The sinnoh id.
        /// </value>
        public int? Sinnoh { get; set; }

        /// <summary>
        /// Gets or sets the einall id (5th generation)
        /// </summary>
        /// <value>
        /// The einall id.
        /// </value>
        public int? Einall { get; set; }

        /// <summary>
        /// Gets or sets the kalos id (6th generation)
        /// </summary>
        /// <value>
        /// The kalos id.
        /// </value>
        public int? Kalos { get; set; }


        /// <summary>
        /// Gets or sets the ranger1 id.
        /// </summary>
        /// <value>
        /// The ranger1 id.
        /// </value>
        public int? Ranger1 { get; set; }

        /// <summary>
        /// Gets or sets the ranger2.
        /// </summary>
        /// <value>
        /// The ranger2.
        /// </value>
        public int? Ranger2 { get; set; }

        /// <summary>
        /// Gets or sets the ranger3.
        /// </summary>
        /// <value>
        /// The ranger3.
        /// </value>
        public int? Ranger3 { get; set; }

        /// <summary>
        /// Gets or sets the mystery dungeon1.
        /// </summary>
        /// <value>
        /// The mystery dungeon1.
        /// </value>
        public int? MysteryDungeon1 { get; set; }

        /// <summary>
        /// Gets or sets the mystery dungeon2.
        /// </summary>
        /// <value>
        /// The mystery dungeon2.
        /// </value>
        public int? MysteryDungeon2 { get; set; }

        /// <summary>
        /// Gets or sets the mystery dungeon3.
        /// </summary>
        /// <value>
        /// The mystery dungeon3.
        /// </value>
        public int? MysteryDungeon3 { get; set; }

        public override string ToString()
        {
            return National.ToString("D3");
        }
    }
}
