// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Table("content_versions")]
    public class ContentVersion
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
        public string Title { get; set; }

        /// <summary>
        /// Plain Content (HTML/Zing/BB)
        /// </summary>
        public string UnparsedContent { get; set; }

        /// <summary>
        /// Parsed Content (only HTML)
        /// </summary>
        public string ParsedContent { get; set; }

        /// <summary>
        /// Content Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of the compiled less Source
        /// </summary>
        [MaxLength(100)]
        public string StylesheetName { get; set; }

        /// <summary>
        /// Less Code for the Stylesheet
        /// </summary>
        public string StylesheetCode { get; set; }

        /// <summary>
        /// Small Teaser for Content Overviews
        /// </summary>
        public string Teaser { get; set; }

        /// <summary>
        /// Editors' Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Id of the User that authored the Version
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Change Note
        /// </summary>
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
