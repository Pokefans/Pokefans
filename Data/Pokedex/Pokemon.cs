using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Pokedex
{
    public enum PokemonGeneration { Generation1 = 1, Generation2 = 2, Generation3 = 3, Generation4 = 4, Generation5 = 5, Generation6 = 6};
    

    [Table("dex_pokemon")]
    public class Pokemon
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public PokemonId PokedexId { get; set; }


        [Column("url")]
        [MaxLength(100)]
        [Index(IsUnique=true)]
        public string Url { get; set; }

        public MultlanguageName Name { get; set; }

        public PokemonGeneration Generation { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public string Abstract { get; set; }

        public string Trivia { get; set; }

        public string TriviaCode { get; set; }

        public string NameDescription { get; set; }

        public int Type1 { get; set; }

        public int? Type2 { get; set; }

        public int? EvolutionBaseId { get; set; }

        public PokemonEvolutionMethod EvolutionMethod { get; set; }

        public int? EvolutionParentId { get; set; }

        public decimal GenderProbabilityFemale { get; set; }

        public decimal GenderProbabilityMale { get; set; }

        public bool CanBreed { get; set; }

        public int? BreedingGroup1Id { get; set; }

        public int? BreedingGroup2Id { get; set; }

        public int? BreedingSteps { get; set; } // steps needed until egg hatched

        public PokemonStatusvalues Values { get; set; }

        public PokemonStatusvalues Base { get; set; }

        public PokemonStatusvalues EffortValues { get; set; }

        public PokemonPokeathlon Pokeathlon { get; set; }

        [MaxLength(50)]
        public string Species { get; set; }

        public decimal Size { get; set; }

        public decimal Weight { get; set; }

        public int ColorId { get; set; }

        public int FootprintId { get; set; }

        public int BodyFormId { get; set; }

        public int EpWin { get; set; }

        public int EpMax { get; set; }

        public int CatchRate { get; set; }

        public int Friendship { get; set; }

        public bool InPokemonConquest { get; set; }

        public int ForumTopicId { get; set; }

        public int? FormItemNeededId { get; set; }

        public string FormChangeDescription { get; set; }

        [ForeignKey("EvolutionBaseId")]
        public Pokemon EvolutionBase { get; set; }

        [ForeignKey("EvolutionParentId")]
        public Pokemon EvolutionParent { get; set; }

        [ForeignKey("BreedingGroup1Id")]
        public PokemonBreedingGroup BreedingGroup1 { get; set; }

        [ForeignKey("BreedingGroup2Id")]
        public PokemonBreedingGroup BreedingGroup2 { get; set; }

        [ForeignKey("ColorId")]
        public PokemonColor Color { get; set; }

        [ForeignKey("BodyFormId")]
        public PokemonBodyForm BodyForm { get; set; }

        [ForeignKey("FootprintId")]
        public PokemonFootprint Footprint { get; set; }

        private ICollection<PokemonStrategy> pokemonStrategies;
        [InverseProperty("PokemonId")]
        public virtual ICollection<PokemonStrategy> PokemonStrategies
        {
            get { return pokemonStrategies ?? (pokemonStrategies = new HashSet<PokemonStrategy>()); }
            set { pokemonStrategies = value; }
        }

        private ICollection<PokemonAbilities> pokemonAbilities;
        [InverseProperty("PokemonId")]
        public virtual ICollection<PokemonAbilities> PokemonAbilities
        {
            get { return pokemonAbilities ?? (pokemonAbilities = new HashSet<PokemonAbilities>()); }
            set { pokemonAbilities = value; }
        }
    }
}
