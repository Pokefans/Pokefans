// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Fanwork
{
    public class FanartChallenge
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int TagId { get; set; }

        public DateTime ExpireDate { get; set; }

        [ForeignKey("TagId")]
        public FanartTag Tag { get; set; }

        private ICollection<Fanart> fanarts;

        public ICollection<Fanart> Fanarts
        {
            get { return fanarts ?? (fanarts = new HashSet<Fanart>()); }
            set { fanarts = value; }
        }
    }
}
