// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public enum PokemonCommonness
    {
        [Display(Name = "Einmalig")]
        Once,
        [Display(Name = "Wiederholend")]
        Recurring,
        [Display(Name = "Selten")]
        Seldom,
        [Display(Name = "Ab und zu")]
        OnceInAWhile,
        [Display(Name = "Oft")]
        Often}

    ;

    public enum PokemonLocationTime
    {
        [Display(Name = "Morgens")]
        Morning,
        [Display(Name = "Tagsüber")]
        Day,
        [Display(Name = "Abends")]
        Evening,
        [Display(Name = "Nacht")]
        Night}

    ;

    public enum PokemonLocationSeason
    {
        [Display(Name = "Winter")]
        Winter,
        [Display(Name = "Frühling")]
        Spring,
        [Display(Name = "Sommer")]
        Summer,
        [Display(Name = "Herbst")]
        Autumn}

    ;

    public class PokemonLocation
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        public int LocationMethodId { get; set; }

        [ForeignKey("LocationMethodId")]
        public PokemonLocationMethod LocationMethod { get; set; }

        public PokemonCommonness Commonness { get; set; }

        public int LevelMin { get; set; }

        public int? LevelMax { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int EditionId { get; set; }

        [ForeignKey("EditionId")]
        public PokemonEdition Edition { get; set; }

        public DateTime LastUpdate { get; set; }

        public int UpdateUserId { get; set; }

        public User UpdateUser { get; set; }

        public PokemonLocationTime Time { get; set; }

        public PokemonLocationSeason Season { get; set; }
    }
}

