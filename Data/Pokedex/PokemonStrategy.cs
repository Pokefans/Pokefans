using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    [Table("dex_strategy")]
    public class PokemonStrategy
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public PokemonGeneration Generation { get; set; }

        public int PokemonId { get; set; }

        public string Text { get; set; }

        public string Code { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }
    }
}
