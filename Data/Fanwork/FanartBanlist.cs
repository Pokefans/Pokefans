// Copyright 2015-2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Fanwork
{
    public class FanartBanlist
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool CanUpload { get; set; }

        public bool CanRate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
