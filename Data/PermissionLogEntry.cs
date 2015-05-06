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
    [Table("system_permission_log")]
    public partial class PermissionLogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("affected_user_id")]
        public int AffectedUserId { get; set; }
        [Required]
        [Column("permission_id")]
        public int PermissionId { get; set; }
        [Required]
        [MaxLength(39)]
        [Column("ip")]
        public string Ip { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("AffectedUserId")]
        public virtual User AffectedUser { get; set; }
    }
}
