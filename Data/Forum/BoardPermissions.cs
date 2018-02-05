using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.Forum
{
    // this is non-transitive.
    // Always overrides Deny
    // Deny overrides Allow
    // Deny, Allow override Default
    // Never overrides All.
    public enum BoardAccess { Never = -2, Deny = -1, Default = 0, Allow = 1, Always = 2 }
    public enum BoardPermissionsets { Role = 0, Guest = 1, Default = 2, Group = 3 }

    public class BoardPermissions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Index("IX_Permissionsets_Role", 3, IsUnique = true)]
        public int? BoardId { get; set; }

        [Index("IX_Permissionsets_Role", 2, IsUnique = true)]
        public int? RoleId { get; set; }

        [Index("IX_Permissionsets_Role", 1, IsUnique = true)]
        public BoardPermissionsets Permissionset { get; set; }

        [Index("IX_Permissionsets_Role", 4, IsUnique = true)]
        public int? GroupId { get; set; }

        public BoardAccess CanRead { get; set; }

        public BoardAccess CanWrite { get; set; }

        public BoardAccess CanModerate { get; set; }

        public BoardAccess CanManage { get; set; }

        [ForeignKey("BoardId")]
        public Board Board { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
