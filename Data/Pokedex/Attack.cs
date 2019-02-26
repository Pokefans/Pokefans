// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;


namespace Pokefans.Data.Pokedex
{
    public enum AttackMode
    {
        Physical,
        Special,
        Status}

    ;

    public class Attack
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public MultilanguageName Name { get; set; }

        public PokemonGeneration Generation { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public virtual PokemonType Type { get; set; }

        public AttackMode Mode { get; set; }

        public int Damage { get; set; }

        public int AttackPoints { get; set; }

        public decimal Accuracy { get; set; }

        public int Priority { get; set; }

        public string Description { get; set; }

        public string DecriptionCode { get; set; }

        public AttackFlags Flags { get; set; }

        public DateTime UpdateTime { get; set; }

        public int UpdateUserId { get; set; }

        [ForeignKey("UpdateUserId")]
        public virtual User UpateUser { get; set; }

        public int ContestTypeId { get; set; }

        [ForeignKey("ContestTypeId")]
        public PokemonContestType ContestType { get; set; }

        public byte ContestAppeal { get; set; }

        public byte ContestJam { get; set; }

        [MaxLength(200)]
        public string ContestDescription { get; set; }

        public decimal DungeonAccuracy { get; set; }

        public decimal DungeonCritical { get; set; }

        public int MysteryDungeonReachId { get; set; }


        private ICollection<AttackTarget> attackTargets;

        [InverseProperty("Attack")]
        public virtual ICollection<AttackTarget> AttackTargets
        {
            get { return attackTargets ?? (attackTargets = new HashSet<AttackTarget>()); }
            set { attackTargets = value; }
        }

        private ICollection<AttackTechnicalMachine> attackTechnicalMachines;

        [InverseProperty("Attack")]
        public virtual ICollection<AttackTechnicalMachine> AttackTechnicalMachines
        {
            get { return attackTechnicalMachines ?? (attackTechnicalMachines = new HashSet<AttackTechnicalMachine>()); }
            set { attackTechnicalMachines = value; }
        }

        private ICollection<AttackHiddenMachine> attackHiddenMachines;

        [InverseProperty("Attack")]
        public virtual ICollection<AttackHiddenMachine> AttackHiddenMachines
        {
            get { return attackHiddenMachines ?? (attackHiddenMachines = new HashSet<AttackHiddenMachine>()); }
            set { attackHiddenMachines = value; }
        }

        internal static void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Attack>().Property(a => a.Name.German).HasColumnName("name_german").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.Chinese).HasColumnName("name_chinese").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.ChineseTranscribed).HasColumnName("name_chinese_trans").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.English).HasColumnName("name_english").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.French).HasColumnName("name_french").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.Japanese).HasColumnName("name_japanese").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.JapnaeseTranscribed).HasColumnName("name_japanese_trans").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.Korean).HasColumnName("name_korean").HasMaxLength(100);
            builder.Entity<Attack>().Property(a => a.Name.KoreanTranscribed).HasColumnName("name_korean_trans").HasMaxLength(100);

            builder.Entity<Attack>().Property(a => a.Flags.Bite).HasColumnName("flag_bite");
            builder.Entity<Attack>().Property(a => a.Flags.ChangeDamage).HasColumnName("flag_change_damage");
            builder.Entity<Attack>().Property(a => a.Flags.Contact).HasColumnName("flag_contact");
            builder.Entity<Attack>().Property(a => a.Flags.HealAmount).HasColumnName("flag_heal_amount");
            builder.Entity<Attack>().Property(a => a.Flags.HealIsAbsorbing).HasColumnName("flag_heal_is_absorbing");
            builder.Entity<Attack>().Property(a => a.Flags.Puls).HasColumnName("flag_puls");
            builder.Entity<Attack>().Property(a => a.Flags.Punch).HasColumnName("flag_punch");
            builder.Entity<Attack>().Property(a => a.Flags.Recoil).HasColumnName("flag_recoil");
            builder.Entity<Attack>().Property(a => a.Flags.RecoilAmount).HasColumnName("flag_recoil_amount");
            builder.Entity<Attack>().Property(a => a.Flags.SelfHeal).HasColumnName("flag_self_heal");
            builder.Entity<Attack>().Property(a => a.Flags.Sound).HasColumnName("flag_sound");
        }
    }
}