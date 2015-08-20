// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data.Pokedex;
using System.Data.Entity;

namespace Pokefans.Data.Wifi
{
    public enum PokemonGender
    {
        [Display(Name = "Männlich")]
        Male,
        [Display(Name = "Weiblich")]
        Female,
        [Display(Name = "Ohne")]
        None}
    ;

    public enum TradingStatus
    {
        Deleted = -2,
        Withdrawn = -1,
        Offer = 0,
        PartnerChosen = 1,
        Completed = 2}

    ;

    public class Offer
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public int? PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        public int Level { get; set; }

        public PokemonGender Gender { get; set; }

        public bool IsOriginalTrainer { get; set; }

        public int PokeballId { get; set; }

        [ForeignKey("PokeballId")]
        public Pokeball Pokeball { get; set; }

        public string Nickname { get; set; }

        public bool IsShiny { get; set; }

        public bool IsEvent { get; set; }

        public bool HasPokerus { get; set; }

        public bool CheatUsesd { get; set; }

        public bool RngUsed { get; set; }

        public bool IsClone { get; set; }

        public int AbilityId { get; set; }

        [ForeignKey("AbilityId")]
        public Ability Ability { get; set; }

        public int Attack1Id { get; set; }

        [ForeignKey("Attack1Id")]
        public Attack Attack1 { get; set; }

        public int Attack2Id { get; set; }

        [ForeignKey("Attack2Id")]
        public Attack Attack2 { get; set; }

        public int Attack3Id { get; set; }

        [ForeignKey("Attack3Id")]
        public Attack Attack3 { get; set; }

        public int Attack4Id { get; set; }

        [ForeignKey("Attack4Id")]
        public Attack Attack4 { get; set; }

        public PokemonStatusvalues AbsoluteValues { get; set; }

        public PokemonStatusvalues DeterValues { get; set; }

        public PokemonStatusvalues EffortValues { get; set; }

        public int NatureId { get; set; }

        [ForeignKey("NatureId")]
        public Nature Nature { get; set; }

        public PokemonGeneration Generation { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateTime { get; set; }

        public TradingStatus Status { get; set; }

        public int ViewCount { get; set; }

        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.Attack).HasColumnName("value_attack");
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.Defense).HasColumnName("value_defense");
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.Speed).HasColumnName("value_speed");
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.SpecialAttack).HasColumnName("value_special_attack");
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.SpeicalDefense).HasColumnName("value_special_defense");
            modelBuilder.Entity<Offer>().Property(a => a.AbsoluteValues.HP).HasColumnName("value_hp");

            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.Attack).HasColumnName("ev_attack");
            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.Defense).HasColumnName("ev_defense");
            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.Speed).HasColumnName("ev_speed");
            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.SpecialAttack).HasColumnName("ev_special_attack");
            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.SpeicalDefense).HasColumnName("ev_special_defense");
            modelBuilder.Entity<Offer>().Property(a => a.EffortValues.HP).HasColumnName("ev_hp");

            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.Attack).HasColumnName("dv_attack");
            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.Defense).HasColumnName("dv_defense");
            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.Speed).HasColumnName("dv_speed");
            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.SpecialAttack).HasColumnName("dv_special_attack");
            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.SpeicalDefense).HasColumnName("dv_special_defense");
            modelBuilder.Entity<Offer>().Property(a => a.DeterValues.HP).HasColumnName("dv_hp");
        }
    }
}