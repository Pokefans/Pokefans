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
    [Table("system_user_persmissions")]
    public partial class UserRole
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("permission_id")]
        public int PermissionId { get; set; }


        [ForeignKey("PermissionId")]
        public virtual Role Permission { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
