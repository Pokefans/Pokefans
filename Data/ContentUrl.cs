// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    public enum UrlStatus { Default, Active, System }

    [Table("content_urls")]
    public class ContentUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index("idx_content_url", IsUnique = true)]
        public string Url { get; set; }

        [Required]
        public int ContentId { get; set; }

        [DefaultValue(UrlStatus.Active)]
        public UrlStatus Status { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}
