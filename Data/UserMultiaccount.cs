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
    public enum MultiaccountStatus { Unspecified = 0, FalsePositive = 1, AllowedMultiaccount = 2}

    [Table("user_multiaccount")]
    public class UserMultiaccount
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserFromId { get; set; }

        [Required]
        public int UserToId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public int ModeratorUserId { get; set; }

        [Required]
        public MultiaccountStatus Status { get; set; }

        public string Note { get; set; }

        [ForeignKey("UserFromId")]
        public User UserFrom { get; set; }

        [ForeignKey("UserToId")]
        public User UserTo { get; set; }

        [ForeignKey("ModeratorUserId")]
        public User Moderator { get; set; }
    }
}
