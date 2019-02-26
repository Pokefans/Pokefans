using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pokefans.Data.UserData
{
    public class UserPage
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public PageStatus Status { get; set; }

        public string Content { get; set; }

        public string ContentCode { get; set; }

        public bool? UsesFullWidth { get; set; }

        public bool CanBeIndexed { get; set; }

        public string Stylesheet { get; set; }

        public string StylesheetCode { get; set; }

        public bool UsesTabs { get; set; }

        [MaxLength(255)]
        public string RedirectUrl { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime CreationTime { get; set; }

        public int ViewCount { get; set; }

        [MaxLength(39)]
        public string LastViewIpAddress { get; set; }
    }

    public enum PageStatus
    {
        Deactivated = -1,
        Unpublished = 0,
        Published = 1}
    ;
}