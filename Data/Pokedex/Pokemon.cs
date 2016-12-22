// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Pokefans.Data.Pokedex
{
    public enum PokemonGeneration
    {
        Generation1 = 1,
        Generation2 = 2,
        Generation3 = 3,
        Generation4 = 4,
        Generation5 = 5,
        Generation6 = 6,
        Generation7 = 7}

    ;


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
        [Index(IsUnique = true)]
        public string Url { get; set; }

        public MultilanguageName Name { get; set; }

        public PokemonGeneration Generation { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public string Abstract { get; set; }

        public string Trivia { get; set; }

        public string TriviaCode { get; set; }

        public string NameDescription { get; set; }

        public int Type1 { get; set; }

        public int? Type2 { get; set; }

        public Nullable<int> EvolutionBaseId { get; set; }

        public int? EvolutionItemId { get; set; }

        [ForeignKey("EvolutionItemId")]
        public Item EvolutionItem { get; set; }

        public int? EvolutionLevel { get; set; }

        public string EvolutionDescription { get; set; }

        public Nullable<int> EvolutionParentId { get; set; }

        public decimal GenderProbabilityFemale { get; set; }

        public decimal GenderProbabilityMale { get; set; }

        public bool CanBreed { get; set; }

        public int? BreedingGroup1Id { get; set; }

        public int? BreedingGroup2Id { get; set; }

        public int? BreedingSteps { get; set; }
        // steps needed until egg hatched

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

        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonStrategy> PokemonStrategies
        {
            get { return pokemonStrategies ?? (pokemonStrategies = new HashSet<PokemonStrategy>()); }
            set { pokemonStrategies = value; }
        }

        private ICollection<PokemonAbility> pokemonAbilities;

        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonAbility> PokemonAbilities
        {
            get { return pokemonAbilities ?? (pokemonAbilities = new HashSet<PokemonAbility>()); }
            set { pokemonAbilities = value; }
        }

        internal static void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Pokemon>()
                .HasOptional(a => a.EvolutionBase)
                .WithMany()
                .HasForeignKey(a => a.EvolutionBaseId);

            builder.Entity<Pokemon>()
                .HasOptional(a => a.EvolutionParent)
                .WithMany()
                .HasForeignKey(a => a.EvolutionParentId);

            builder.Entity<Pokemon>().Property(a => a.Name.German).HasColumnName("name_german").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.Chinese).HasColumnName("name_chinese").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.ChineseTranscribed).HasColumnName("name_chinese_trans").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.English).HasColumnName("name_english").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.French).HasColumnName("name_french").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.Japanese).HasColumnName("name_japanese").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.JapnaeseTranscribed).HasColumnName("name_japanese_trans").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.Korean).HasColumnName("name_korean").HasMaxLength(100);
            builder.Entity<Pokemon>().Property(a => a.Name.KoreanTranscribed).HasColumnName("name_korean_trans").HasMaxLength(100);

            builder.Entity<Pokemon>().Property(a => a.Values.Attack).HasColumnName("value_attack");
            builder.Entity<Pokemon>().Property(a => a.Values.Defense).HasColumnName("value_defense");
            builder.Entity<Pokemon>().Property(a => a.Values.Speed).HasColumnName("value_speed");
            builder.Entity<Pokemon>().Property(a => a.Values.SpecialAttack).HasColumnName("value_special_attack");
            builder.Entity<Pokemon>().Property(a => a.Values.SpecialDefense).HasColumnName("value_special_defense");
            builder.Entity<Pokemon>().Property(a => a.Values.HP).HasColumnName("value_hp");

            builder.Entity<Pokemon>().Property(a => a.EffortValues.Attack).HasColumnName("ev_attack");
            builder.Entity<Pokemon>().Property(a => a.EffortValues.Defense).HasColumnName("ev_defense");
            builder.Entity<Pokemon>().Property(a => a.EffortValues.Speed).HasColumnName("ev_speed");
            builder.Entity<Pokemon>().Property(a => a.EffortValues.SpecialAttack).HasColumnName("ev_special_attack");
            builder.Entity<Pokemon>().Property(a => a.EffortValues.SpecialDefense).HasColumnName("ev_special_defense");
            builder.Entity<Pokemon>().Property(a => a.EffortValues.HP).HasColumnName("ev_hp");

            builder.Entity<Pokemon>().Property(a => a.Base.Attack).HasColumnName("base_attack");
            builder.Entity<Pokemon>().Property(a => a.Base.Defense).HasColumnName("base_defense");
            builder.Entity<Pokemon>().Property(a => a.Base.Speed).HasColumnName("base_speed");
            builder.Entity<Pokemon>().Property(a => a.Base.SpecialAttack).HasColumnName("base_special_attack");
            builder.Entity<Pokemon>().Property(a => a.Base.SpecialDefense).HasColumnName("base_special_defense");
            builder.Entity<Pokemon>().Property(a => a.Base.HP).HasColumnName("base_hp");

            builder.Entity<Pokemon>().Property(a => a.Pokeathlon.Force).HasColumnName("pokeathlon_force");
            builder.Entity<Pokemon>().Property(a => a.Pokeathlon.Jump).HasColumnName("pokeathlon_jump");
            builder.Entity<Pokemon>().Property(a => a.Pokeathlon.Speed).HasColumnName("pokeathlon_speed");
            builder.Entity<Pokemon>().Property(a => a.Pokeathlon.Sustain).HasColumnName("pokeathlon_sustain");
            builder.Entity<Pokemon>().Property(a => a.Pokeathlon.Technique).HasColumnName("pokeathlon_technique");

            builder.Entity<Pokemon>().Property(a => a.PokedexId.Einall).HasColumnName("id_einall");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Hoenn).HasColumnName("id_hoenn");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Johto).HasColumnName("id_johto");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Kanto).HasColumnName("id_kanto");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Kalos).HasColumnName("id_kalos");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.MysteryDungeon1).HasColumnName("id_dungeon1");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.MysteryDungeon2).HasColumnName("id_dungeon2");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.MysteryDungeon3).HasColumnName("id_dungeon3");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.National).HasColumnName("id_national");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Ranger1).HasColumnName("id_ranger1");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Ranger2).HasColumnName("id_ranger2");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Ranger3).HasColumnName("id_ranger3");
            builder.Entity<Pokemon>().Property(a => a.PokedexId.Sinnoh).HasColumnName("id_sinnoh");
        }
    }
}
