// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    [Table("content_categories")]
    public class ContentCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AreaName { get; set; }

        public int ForumId { get; set; }

        public int SidebarContentId { get; set; }

        public int OrderingPosition { get; set; }

        [ForeignKey("SidebarContentId")]
        public virtual Content SidebarContent { get; set; }

    }
}
