// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// Default = Default Content Url
    /// Active  = Alternative Content Url
    /// System  = System assigned Content Url 
    /// </summary>
    public enum UrlStatus { Default, Active, System }

    /// <summary>
    /// 
    /// </summary>
    [Table("content_urls")]
    public class ContentUrl
    {
        /// <summary>
        /// Unique Id for the ContentUrl Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Unique Url
        /// </summary>
        [Required]
        [Index("idx_content_url", IsUnique = true)]
        public string Url { get; set; }

        /// <summary>
        /// Id of the corresponding Content Object
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Url Status
        /// </summary>
        [DefaultValue(UrlStatus.Active)]
        public UrlStatus Status { get; set; }

        /// <summary>
        /// Corresponding Content Object
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}
