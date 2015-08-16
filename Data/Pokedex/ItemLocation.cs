using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    class ItemLocation
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int LocationId { get; set; }

        public string Description { get; set; }

        public int EditionId { get; set; }

        [ForeignKey("EditionId")]
        public PokemonEdition Edition { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }
}
