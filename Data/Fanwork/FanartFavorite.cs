// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Fanwork
{
    public class FanartFavorite
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int FanartId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("FanartId")]
        public Fanart Fanart { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

