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
    [Table("system_permissions")]
    public partial class Permission
    {

        public Permission()
        {
            this.PermissionLogs = new HashSet<PermissionLogEntry>();
            this.Children = new HashSet<Permission>();
            this.UserPermissions = new HashSet<UserPermission>();
        }

        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(45)]
        [Column("name")]
        [Index(IsUnique=true)]
        public string Name { get; set; }

        [MaxLength(45)]
        [Column("friendly_name")]
        [Index(IsUnique = true)]
        public string FriendlyName { get; set; }

        [Column("metapermission_id")]
        public Nullable<int> MetapermissionId { get; set; }


        [InverseProperty("Permission")]
        public virtual ICollection<PermissionLogEntry> PermissionLogs { get; set; }

        [InverseProperty("Metapermission")]
        public virtual ICollection<Permission> Children { get; set; }
        
        [ForeignKey("MetapermissionId")]
        public virtual Permission Metapermission { get; set; }

        [InverseProperty("Permission")]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }

    }
}
