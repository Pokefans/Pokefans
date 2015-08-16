// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// Default = Default Content Url
    /// Alternative  = Alternative Content Url
    /// System  = System assigned Content Url 
    /// </summary>
    public enum UrlType
    {
        [Display(Name = "Primär-URL")]
        Default,

        [Display(Name = "Alternativ-URL")]
        Alternative,

        [Display(Name = "System-URL")]
        System
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("content_urls")]
    public partial class ContentUrl
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
        [MaxLength(90)]
        [Index("idx_content_url", IsUnique = true)]
        public string Url { get; set; }

        /// <summary>
        /// Id of the corresponding Content Object
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Url Type
        /// </summary>
        [DefaultValue(UrlType.Alternative)]
        public UrlType Type { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Corresponding Content Object
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}
