// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data
{
    [Table("user_advertising")]
    public class UserAdvertising
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("url")]
        [Required]
        public string Url { get; set; }

        [Column("screenshot_url")]
        public string ScreenshotUrl { get; set; }

        [Column("report_time")]
        [Required]
        public DateTime ReportTime { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        [Column("advertising_form_id")]
        [Required]
        public int AdvertisingFormId { get; set; }

        [Column("advertising_from_id")]
        public int AdvertisingFromId { get; set; }

        [Column("advertising_to")]
        public int? AdvertisingToId { get; set; }

        [ForeignKey("AdvertisingFormId")]
        public UserAdvertisingForm AdvertisingForm { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; }

        [ForeignKey("AdvertisingToId")]
        public User AdvertisingTo { get; set; }

        [ForeignKey("AdvertisingFromId")]
        public User AdvertisingFrom { get; set; }
    }
}
