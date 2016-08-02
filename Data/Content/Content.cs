// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Pokefans.Data
{
    /// <summary>
    /// Discarded     = Will not be published, abandoned work
    /// WorkInProcess = Content unfinished, work in process.
    /// Ready         = Content finished, waiting for review and publish
    /// Published     = self-explanatory
    /// </summary>
    public enum ContentStatus
    {
        [Display(Name = "In Arbeit")]
        WorkInProcess,

        [Display(Name = "Fertig")]
        Ready,

        [Display(Name = "Veröffentlicht")]
        Published,

        [Display(Name = "Verworfen")]
        Discarded
    }

    /// <summary>
    /// Normal          = Will be listed on home page
    /// NotOnHomePage   = Will be excluded from home page
    /// FixedOnHomePage = Will be fixed on home page
    /// </summary>
    public enum HomePageOptions
    {
        [Display(Name = "Normal")]
        Normal,

        [Display(Name = "Ausgeblendet")]
        NotOnHomePage,

        [Display(Name = "Fixiert")]
        FixedOnHomePage
    }

    /// <summary>
    /// Article         = normal article
    /// News            = news entry
    /// Special         = sidebars, teasers...
    /// Boilerplate     = boilerplate for use in other articles
    /// </summary>
    public enum ContentType
    {
        [Display(Name = "Artikel")]
        Article,

        [Display(Name = "Newsmeldung")]
        News,

        [Display(Name = "Spezialelement")]
        Special,

        [Display(Name = "Textbaustein")]
        Boilerplate
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("content")]
    public partial class Content
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
        public int? CategoryId { get; set; }

        /// <summary>
        /// Id of the Permission required to edit this Content
        /// </summary>
        public int? EditPermissionId { get; set; }

        /// <summary>
        /// Publication Options for the Home Page
        /// </summary>
        [DefaultValue(HomePageOptions.Normal)]
        public HomePageOptions HomePageOptions { get; set; }

        /// <summary>
        /// Number of Views
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Time of Publication
        /// </summary>
        public DateTime Published { get; set; }

        /// <summary>
        /// Id of the publicator
        /// </summary>
        public int? PublishedByUserId { get; set; }

        /// <summary>
        /// Time of last Change
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Time of creation
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Id of the Creator
        /// </summary>
        [Required]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// Id of the default Url
        /// </summary>
        public int? DefaultUrlId { get; set; }

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

        /// <summary>
        /// Permission required to edit this Content
        /// </summary>
        [ForeignKey("EditPermissionId")]
        public virtual Role EditPermission { get; set; }

        private ICollection<ContentUrl> _urls;

        /// <summary>
        /// All urls for this content
        /// </summary>
        [InverseProperty("Content")]
        public ICollection<ContentUrl> Urls
        {
            get { return _urls ?? (_urls = new HashSet<ContentUrl>()); }
            set { _urls = value; }
        }

        private ICollection<ContentFeedback> _feedback;

        /// <summary>
        /// All feedback for this content
        /// </summary>
        [InverseProperty("Content")]
        public ICollection<ContentFeedback> Feedbacks
        {
            get { return _feedback ?? (_feedback = new HashSet<ContentFeedback>()); }
            set { _feedback = value; }
        }

        private ICollection<ContentVersion> _versions;

        /// <summary>
        /// All versions of this content
        /// </summary>
        [InverseProperty("Content")]
        public ICollection<ContentVersion> Versions
        {
            get { return _versions ?? (_versions = new HashSet<ContentVersion>()); }
            set { _versions = value; }
        }

        private ICollection<ContentBoilerplate> _boilerplates;

        /// <summary>
        /// All boilerplates used in this content
        /// </summary>
        [InverseProperty("Content")]
        public ICollection<ContentBoilerplate> Boilerplates
        {
            get { return _boilerplates ?? (_boilerplates = new HashSet<ContentBoilerplate>()); }
            set { _boilerplates = value; }
        }

        private ICollection<ContentBoilerplate> _boilerplatesUsed;

        /// <summary>
        /// All contents this content is used for as a boilerplate
        /// </summary>
        [InverseProperty("Boilerplate")]
        public ICollection<ContentBoilerplate> BoilerplatesUsed
        {
            get { return _boilerplatesUsed ?? (_boilerplatesUsed = new HashSet<ContentBoilerplate>()); }
            set { _boilerplatesUsed = value; }
        }

        [NotMapped]
        public string StylesheetUrl => "//static." + ConfigurationManager.AppSettings["Domain"] + "/stylesheets/content/" + this.Id + ".css";
    }
}
