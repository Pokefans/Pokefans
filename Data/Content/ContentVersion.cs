// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Table("content_versions")]
    public partial class ContentVersion
    {
        /// <summary>
        /// Unique Id for the ContentVersion Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Id of the corresponding Content Object
        /// </summary>
        [Required]
        public int ContentId { get; set; }
        
        /// <summary>
        /// Current Version
        /// </summary>
        [Required]
        public int Version { get; set; }

        /// <summary>
        /// Title/Headline
        /// </summary>
        [MaxLength(255, ErrorMessage = "Der Titel darf maximal 255 Zeichen lang sein.")]
        [DefaultValue("")]
        public string Title { get; set; }

        /// <summary>
        /// Plain Content (HTML/Zing/BB)
        /// </summary>
        [DefaultValue("")]
        public string UnparsedContent { get; set; }

        /// <summary>
        /// Parsed Content (only HTML)
        /// </summary>
        [DefaultValue("")]
        public string ParsedContent { get; set; }

        /// <summary>
        /// Content Description
        /// </summary>
        [DefaultValue("")]
        public string Description { get; set; }

        /// <summary>
        /// Compiled less Source
        /// </summary>
        [DefaultValue("")]
        public string StylesheetCss { get; set; }

        /// <summary>
        /// Less Code for the Stylesheet
        /// </summary>
        [DefaultValue("")]
        public string StylesheetCode { get; set; }

        /// <summary>
        /// Small Teaser for Content Overviews
        /// </summary>
        [DefaultValue("")]
        public string Teaser { get; set; }

        /// <summary>
        /// Editors' Notes
        /// </summary>
        [DefaultValue("")]
        public string Notes { get; set; }

        /// <summary>
        /// Id of the User that authored the Version
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Change Note
        /// </summary>
        [DefaultValue("")]
        public string Note { get; set; }

        /// <summary>
        /// Time of Change
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Magnificance of the last Update
        /// </summary>
        public double UpdateMagnificance { get; set; }

        /// <summary>
        /// Number of changed/added Characters in the last Update
        /// </summary>
        public int UpdateCharsChanged { get; set; }

        /// <summary>
        /// Number of deleted Characters in the last Update
        /// </summary>
        public int UpdateCharsDeleted { get; set; }

        /// <summary>
        /// Corresponding Content Object
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        /// <summary>
        /// User that authored the Version
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
