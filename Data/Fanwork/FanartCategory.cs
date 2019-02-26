// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.Fanwork
{
    public class FanartCategory
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }
        
        /// <summary>
        /// Maximum File Size (in Byte)
        /// </summary>
        [Required]
        public int MaxFileSize { get; set; }

        /// <summary>
        /// Maximum Width and height. 50 means 50x50.
        /// </summary>
        [Required]
        public int MaximumDimension { get; set; }

        public int Order { get; set; }
    }
}

