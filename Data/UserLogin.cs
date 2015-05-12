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
    [Table("system_user_logins")]
    public partial class UserLogin
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column("time")]
        [Required]
        public DateTime Time { get; set; }
        
        [Column("ip")]
        [MaxLength(39)]
        [Required]
        public string Ip { get; set; }
        
        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("success")]
        [Required]
        public bool Success { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


    }
}
