// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Pokefans.Data
{
    [Table("system_permissions")]
    public partial class Role : IRole<int>
    {

        public Role()
        {
        }

        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(45)]
        [Column("name")]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(45)]
        [Column("friendly_name")]
        [Index(IsUnique = true)]
        public string FriendlyName { get; set; }

        [Column("metapermission_id")]
        public Nullable<int> MetapermissionId { get; set; }


        private ICollection<RoleLogEntry> roleLogs;
        [InverseProperty("Permission")]
        public virtual ICollection<RoleLogEntry> RoleLogs
        {
            get { return roleLogs ?? (roleLogs = new HashSet<RoleLogEntry>()); }
            set { roleLogs = value; }
        }

        private ICollection<Role> children;
        [InverseProperty("Metapermission")]
        public virtual ICollection<Role> Children
        {
            get { return children ?? (children = new HashSet<Role>()); }
            set { children = value; }
        }

        [ForeignKey("MetapermissionId")]
        public virtual Role Metapermission { get; set; }

        private ICollection<UserRole> userRoles;
        [InverseProperty("Permission")]
        public virtual ICollection<UserRole> UserRoles
        {
            get { return userRoles ?? (userRoles = new HashSet<UserRole>()); }
            set { userRoles = value; }
        }

    }
}
