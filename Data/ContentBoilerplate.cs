// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    [Table("content_boilerplates")]
    public class ContentBoilerplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ContentId { get; set; }

        public int BoilerplateId { get; set; }
        
        public string ContentBoilerplateName { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [ForeignKey("BoilerplateId")]
        public virtual Content Boilerplate { get; set; }
    }
}
