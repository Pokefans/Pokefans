// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// Discarded     = Will not be published, abandoned work
    /// WorkInProcess = Content unfinished, work in process.
    /// Ready         = Content finished, waiting for review and publish
    /// Published     = self-explanatory
    /// </summary>
    public enum ContentStatus { WorkInProcess, Ready, Published, Discarded }

    /// <summary>
    /// Normal          = Will be listed on home page
    /// NotOnHomePage   = Will be excluded from home page
    /// FixedOnHomePage = Will be fixed on home page
    /// </summary>
    public enum HomePageOptions { Normal, NotOnHomePage, FixedOnHomePage }

    /// <summary>
    /// Article         = normal article
    /// News            = news entry
    /// Special         = sidebars, teasers...
    /// Boilerplate     = boilerplate for use in other articles
    /// </summary>
    public enum ContentType { Article, News, Special, Boilerplate }

    /// <summary>
    /// 
    /// </summary>
    [Table("content")]
    public class Content
    {
        /// <summary>
        /// Unique Id for the Content Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        /// Publication Status
        /// </summary>
        [DefaultValue(ContentStatus.WorkInProcess)]
        public ContentStatus Status { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        [DefaultValue(ContentType.Article)]
        public ContentType Type { get; set; }

        /// <summary>
        /// Current Version of the Content
        /// </summary>
        [Required]
        public int Version { get; set; }

        /// <summary>
        /// Id of the corresponding ContentCategory Object
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Id of the Permission required to edit this Content
        /// </summary>
        public int EditPermissionId { get; set; }

        /// <summary>
        /// Publication Options for the Home Page
        /// </summary>
        [DefaultValue(HomePageOptions.Normal)]
        public HomePageOptions HomePageOptions { get; set; }

        /// <summary>
        /// Number of Views
        /// </summary>
        public string ViewCount { get; set; }

        /// <summary>
        /// Time of Publication
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// Id of the publicator
        /// </summary>
        public int PublishedByUserId { get; set; }

        /// <summary>
        /// Time of last Change
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Id of the Creator
        /// </summary>
        [Required]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// Id of the default Url
        /// </summary>
        public int DefaultUrlId { get; set; }

        /// <summary>
        /// Category the Content is published in
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual ContentCategory Category { get; set; }

        /// <summary>
        /// User that published the Content
        /// </summary>
        [ForeignKey("PublishedByUserId")]
        public virtual User PublishedByUser { get; set; }

        /// <summary>
        /// User that created the Content
        /// </summary>
        [ForeignKey("AuthorUserId")]
        public virtual User Author { get; set; }

        /// <summary>
        /// Default Url
        /// </summary>
        [ForeignKey("DefaultUrlId")]
        public virtual ContentUrl DefaultUrl { get; set; }
    }
}
