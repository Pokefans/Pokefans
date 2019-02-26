// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Pokefans.Data.Pokedex
{
    [Table("dex_ability")]
    public class Ability
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public MultilanguageName Name { get; set; }

        [MaxLength(255)]
        public string IngameDescription { get; set; }

        [MaxLength(255)]
        public string IngameDescriptionEnglish { get; set; }

        public string Abstract { get; set; }

        public string AbstractCode { get; set; }

        public string BattleDescription { get; set; }

        public string BattleDescriptionCode { get; set; }

        public string ExternalDescription { get; set; }

        public string ExternalDescriptionCode { get; set; }

        public string MysteryDungeonDescription { get; set; }

        public string MysteryDungeonDescriptionCode { get; set; }

        public PokemonGeneration Generation { get; set; }

        public string EmeraldDescription { get; set; }

        public string EmeraldDescriptionCode { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int LastUpdateUserId { get; set; }

        [ForeignKey("LastUpdateUserId")]
        public User LastUpdateUser { get; set; }

        internal static void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Ability>().Property(a => a.Name.German).HasColumnName("name_german").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.Chinese).HasColumnName("name_chinese").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.ChineseTranscribed).HasColumnName("name_chinese_trans").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.English).HasColumnName("name_english").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.French).HasColumnName("name_french").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.Japanese).HasColumnName("name_japanese").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.JapnaeseTranscribed).HasColumnName("name_japanese_trans").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.Korean).HasColumnName("name_korean").HasMaxLength(100);
            builder.Entity<Ability>().Property(a => a.Name.KoreanTranscribed).HasColumnName("name_korean_trans").HasMaxLength(100);
        }
    }
}
