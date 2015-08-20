// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Pokedex
{
    public class PokemonImage
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int PokemonId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        [MaxLength(255)]
        public string FilePath { get; set; }

        public ushort FileSize { get; set; }

        public ushort Width { get; set; }

        public ushort Height { get; set; }

        public string Description { get; set; }

        [Range(0, 999)]
        public ushort Sorting { get; set; }

        public int UploadUserId { get; set; }

        [ForeignKey("UploadUserId")]
        public User UploadUser { get; set; }

        public DateTime UploadTime { get; set; }

        [MaxLength(255)]
        public string OriginalFileName { get; set; }
    }
}

