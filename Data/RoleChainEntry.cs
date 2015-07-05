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
    [Table("system_role_chain")]
    public class RoleChainEntry
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("parent_role_id")]
        public int ParentRoleId { get; set; }

        [Required]
        [Column("child_role_id")]
        public int ChildRoleId { get; set; }

        [ForeignKey("ParentRoleId")]
        public virtual Role Parent { get; set; }

        [ForeignKey("ChildRoleId")]
        public virtual Role Child { get; set; }
    }
}
