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

    public class Item
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(25)]
        public string Url { get; set; }

        public MultilanguageName Name { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        [MaxLength(255)]
        public string IngameDescription { get; set; }

        [MaxLength(25)]
        public string ImageUrl { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int UpdateUserId { get; set; }

        [ForeignKey("UpdateUserId")]
        public User UpdateUser { get; set; }

        public int RequestCount { get; set; }

        public int SearchCount { get; set; }

        internal static void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Item>().Property(a => a.Name.German).HasColumnName("name_german").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.Chinese).HasColumnName("name_chinese").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.ChineseTranscribed).HasColumnName("name_chinese_trans").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.English).HasColumnName("name_english").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.French).HasColumnName("name_french").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.Japanese).HasColumnName("name_japanese").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.JapnaeseTranscribed).HasColumnName("name_japanese_trans").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.Korean).HasColumnName("name_korean").HasMaxLength(100);
            builder.Entity<Item>().Property(a => a.Name.KoreanTranscribed).HasColumnName("name_korean_trans").HasMaxLength(100);
        }
    }
}