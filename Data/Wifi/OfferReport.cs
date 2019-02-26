// copyright 2019 the pokefans authors. See copying.md for legal info
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Wifi
{
    public class OfferReport
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string Comment { get; set; }

        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

        public DateTime ReportedOn { get; set; }

        public bool Resolved { get; set; }
    }
}
