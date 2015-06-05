// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Table("content_categories")]
    public class ContentCategory
    {
        /// <summary>
        /// Unique Id for the ContentCategory Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Name for the corresponding MVC Area
        /// </summary>
        [Required]
        public string AreaName { get; set; }

        /// <summary>
        /// Id of the corresponding Subforum
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// Id of the Content operating as a Sidebar
        /// </summary>
        public int SidebarContentId { get; set; }

        /// <summary>
        /// Position for Category Ordering
        /// </summary>
        public int OrderingPosition { get; set; }

        /// <summary>
        /// Content operating as a Sidebar
        /// </summary>
        [ForeignKey("SidebarContentId")]
        public virtual Content SidebarContent { get; set; }

    }
}
