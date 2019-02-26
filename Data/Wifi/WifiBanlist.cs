// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Wifi
{
    public class WifiBanlist
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool CanAddOffers { get; set; }

        public DateTime? ExpireAddOffers { get; set; }

        public bool CanInterest { get; set; }

        public DateTime? ExpireInterest { get; set; }

        public string BanReason { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
