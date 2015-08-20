// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data;

namespace Pokefans.Data.Contents
{
    public class LexiconEntry
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        public char Letter { get; set; }

        public string Description { get; set; }

        public string DescriptionCode { get; set; }

        public ContentStatus Status { get; set; }
    }
}