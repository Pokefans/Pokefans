// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Fanwork
{
    public class FanartTags
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int FanartId { get; set; }

        public int TagId { get; set; }

        [ForeignKey("FanartId")]
        public Fanart Fanart { get; set; }

        [ForeignKey("TagId")]
        public FanartTag Tag { get; set; }
    }
}

