using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Fanwork
{
    public class FanartRating
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public decimal Rating { get; set; }

        public int FanartId { get; set; }

        public int RatingUserId { get; set; }

        public DateTime RatingTime { get; set; }

        [MaxLength(47)]
        public string RatingIp { get; set; }

        [ForeignKey("RatingUserId")]
        public User RatingUser { get; set; }

        [ForeignKey("FanartId")]
        public Fanart Fanart { get; set; }
    }
}
