// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pokefans.Data;
using Pokefans.Util.Validation;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentEditViewModel
    {
        public int? ContentId { get; set; }

        [DefaultValue(false)]
        public bool Saved { get; set; }

        public bool IsContentAdministrator { get; set; }

        public IEnumerable<SelectListItem> ContentStatusList { get; set; }

        public IEnumerable<SelectListItem> ContentPermissionList { get; set; }

        [Display(Name = "Notwendige Berechtigung zum Bearbeiten")]
        public int? PermissionId { get; set; }

        [Display(Name = "Dateiname in URL")]
        [RegularExpression(@"[a-zA-Z0-9_\-/]+", ErrorMessage = "Die URL darf nur alphanumerische Zeichen und Unter- sowie Bindestriche enthalten.")]
        public string Url { get; set; }
            
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        [Required(ErrorMessage = "Ein Titel muss angegeben werden.")]
        [MaxLength(45, ErrorMessage = "Der Titel darf maximal 45 Zeichen lang sein.")]
        [Display(Name = "Titel")]
        [NoJavascript(ErrorMessage = "Der Titel darf kein Javascript enthalten.")]
        public string Title { get; set; }

        [Display(Name = "Inhalt")]
        [NoJavascript(ErrorMessage = "Der Inhalt darf kein Javascript enthalten.")]
        public string UnparsedContent { get; set; }

        [Display(Name = "Artikelspezifisches Stylesheet")]
        public string StylesheetCode { get; set; }

        [MaxLength(255, ErrorMessage = "Die Beschreibung darf maximal 255 Zeichen lang sein.")]
        [Display(Name = "Zusammenfassung")]
        [NoJavascript(ErrorMessage = "Die Beschreibung darf kein Javascript enthalten.")]
        public string Description { get; set; }

        [MaxLength(255, ErrorMessage = "Die Beschreibung darf maximal 255 Zeichen lang sein.")]
        [Display(Name = "Kurzbeschreibung")]
        [NoJavascript(ErrorMessage = "Die Beschreibung darf kein Javascript enthalten.")]
        public string Teaser { get; set; }

        [Display(Name = "Notizen")]
        [NoJavascript(ErrorMessage = "Die Notizen dürfen kein Javascript enthalten.")]
        public string Notes { get; set; }

        [Display(Name = "Kategorie")]
        public int? CategoryId { get; set; }

        [DefaultValue(ContentType.Article)]
        [Display(Name = "Klassifizierung")]
        public ContentType Type { get; set; }

        [DefaultValue(ContentStatus.WorkInProcess)]
        [Display(Name = "Status")]
        public ContentStatus Status { get; set; }

        [DefaultValue(HomePageOptions.Normal)]
        [Display(Name = "Startseitenoption")]
        public HomePageOptions HomePageOptions { get; set; }

        public IEnumerable<ContentCategory> Categories { get; set; }

        /// <summary>
        /// Update the Model to match the given Content Object
        /// </summary>
        /// <param name="content"></param>
        public void UpdateFromContent(Content content)
        {
            ContentId = content.Id;
            Title = content.Title ?? "";
            UnparsedContent = content.UnparsedContent ?? "";
            StylesheetCode = content.StylesheetCode ?? "";
            Description = content.Description ?? "";
            Teaser = content.Teaser ?? "";
            Notes = content.Notes ?? "";
            CategoryId = content.CategoryId;
            Type = content.Type;
            Status = content.Status;
            HomePageOptions = content.HomePageOptions;
            PermissionId = content.EditPermissionId;
            Url = content.DefaultUrl.Url;
        }

        /// <summary>
        /// Update the given Content Object with the Model Data
        /// </summary>
        /// <param name="content"></param>
        public void UpdateContentMeta(Content content)
        {
            content.Title = Title ?? "";
            content.UnparsedContent = UnparsedContent ?? "";
            content.StylesheetCode = StylesheetCode ?? "";
            content.Description = Description ?? "";
            content.Teaser = Teaser ?? "";
            content.Notes = Notes ?? "";
            content.CategoryId = CategoryId;
            content.Type = Type;
            content.Status = Status;
            content.HomePageOptions = HomePageOptions;
        }

        /// <summary>
        /// Create a new Model from the given Content Object
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ContentEditViewModel FromContent(Content content)
        {
            var model = new ContentEditViewModel();
            model.UpdateFromContent(content);

            return model;
        }

        /// <summary>
        /// Update the given ContentVersion Object with the  Model Data
        /// </summary>
        /// <param name="content"></param>
        public void UpdateContentVersion(ContentVersion content)
        {
            content.Title = Title ?? "";
            content.UnparsedContent = UnparsedContent ?? "";
            content.StylesheetCode = StylesheetCode ?? "";
            content.Description = Description ?? "";
            content.Teaser = Teaser ?? "";
            content.Notes = Notes ?? "";
        }
    }
}