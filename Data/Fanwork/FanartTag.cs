// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Pokefans.Data.Fanwork
{
    public class FanartTag
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        private ICollection<FanartTags> tags;

        [InverseProperty("Tag")]
        public virtual ICollection<FanartTags> Fanarts
        {
            get { return tags ?? (tags = new HashSet<FanartTags>()); }
            set { tags = value; }
        }
    }
}

