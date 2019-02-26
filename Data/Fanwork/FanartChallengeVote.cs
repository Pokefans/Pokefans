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
    public class FanartChallengeVote
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int FanartId { get; set; }

        public int UserId { get; set; }

        public int ChallengeId { get; set; }

        public DateTime Timestamp { get; set; }

        [MaxLength(47)]
        public string VoteIp { get; set; }

        [ForeignKey("FanartId")]
        public Fanart Fanart { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ChallengeId")]
        public FanartChallenge Challenge { get; set; }
    }
}
