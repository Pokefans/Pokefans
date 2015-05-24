// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    [Table("content_versions")]
    public class ContentVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ContentId { get; set; }
        
        [Required]
        public int Version { get; set; }

        [MaxLength(255, ErrorMessage = "Der Titel darf maximal 255 Zeichen lang sein.")]
        public string Title { get; set; }

        public string UnparsedContent { get; set; }

        public string ParsedContent { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Note { get; set; }

        public DateTime Updated { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
