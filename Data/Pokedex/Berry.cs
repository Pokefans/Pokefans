// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Pokefans.Data.Pokedex
{
    public class Berry
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int IngameId { get; set; }

        public MultilanguageName Name { get; set; }

        [MaxLength(100)]
        public string NameOrigin { get; set; }

        public int AttackTypeId { get; set; }

        [ForeignKey("AttackTypeId")]
        public PokemonType AttackType { get; set; }

        public int AttackDamage { get; set; }

        public int Quality { get; set; }

        public int Texture { get; set; }

        public decimal Size { get; set; }

        public int HarvestMin { get; set; }

        public int HarvestMax { get; set; }

        public int GrowTime { get; set; }

        public int WateringTime { get; set; }

        public int Spicy { get; set; }

        public int Dry { get; set; }

        public int Sweet { get; set; }

        public int Bitter { get; set; }

        public int Sour { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        internal static void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Berry>().Property(a => a.Name.German).HasColumnName("name_german").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.Chinese).HasColumnName("name_chinese").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.ChineseTranscribed).HasColumnName("name_chinese_trans").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.English).HasColumnName("name_english").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.French).HasColumnName("name_french").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.Japanese).HasColumnName("name_japanese").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.JapnaeseTranscribed).HasColumnName("name_japanese_trans").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.Korean).HasColumnName("name_korean").HasMaxLength(100);
            builder.Entity<Berry>().Property(a => a.Name.KoreanTranscribed).HasColumnName("name_korean_trans").HasMaxLength(100);

        }
    }
}

