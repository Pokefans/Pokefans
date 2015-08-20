// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Table("content_boilerplates")]
    public partial class ContentBoilerplate
    {
        /// <summary>
        /// Unique Id for the ContentBoilerplate Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Id of the Content Object the Boilerplate is embedded in
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Id of the Content Object that is embedded
        /// </summary>
        [Required]
        public int BoilerplateId { get; set; }
        
        /// <summary>
        /// Name for the Boilerplate
        /// </summary>
        [Required]
        public string ContentBoilerplateName { get; set; }

        /// <summary>
        /// Content Object the Boilerplate is embedded in
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        /// <summary>
        /// Content Object that is embedded
        /// </summary>
        [ForeignKey("BoilerplateId")]
        public virtual Content Boilerplate { get; set; }
    }
}
