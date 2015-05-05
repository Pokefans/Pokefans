// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pokefans.Data.Attributes;

namespace Pokefans.Data
{
    [Table("system_users")]
    public partial class User
    {
        public User()
        {
            this.PermissionLogs = new HashSet<PermissionLogEntry>();
            this.GivenPermissionLogs = new HashSet<PermissionLogEntry>();
            this.Logins = new HashSet<UserLogin>();
            this.Permissions = new HashSet<UserPermission>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Du musst einen Benutzernamen angeben")]
        [MaxLength(45, ErrorMessage = "Dein Benutzername darf maximal 45 Zeichen lang sein.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9-_ ]{0,43}[a-zA-Z0-9]$", ErrorMessage = "Dein Benutzername darf nur aus Großbuchstaben, Kleinbuchstaben, Bindestrich (-) und Unterstich(_) bestehen. Außerdem muss er mit einem Buchstaben beginnen und darf nicht mit Bindestrich oder Unterstrich aufhören.")]
        [Column("name", TypeName = "VARCHAR")]
        [Index]
        public string Name { get; set; }

        [Required]
        [Column("registered")]
        public DateTime Registered { get; set; }

        [Required]
        [MaxLength(39)]
        [Column("registered_ip")]
        public string RegisteredIp { get; set; }

        [Required]
        [MaxLength(45)]
        [Column("url")]
        [Index]
        public string Url { get; set; }

        [Required]
        [Column("status")]
        public byte Status { get; set; }

        [Column("ban_reason")]
        public string BanReason { get; set; }

        [Column("ban_time")]
        public Nullable<DateTime> BanTime { get; set; }

        [MaxLength(45)]
        [Column("rank")]
        public string Rank { get; set; }

        [MaxLength(9)]
        [Column("color")]
        public string Color { get; set; }

        [Column("unread_notifications")]
        public Nullable<short> UnreadNotificationCount { get; set; }

        [Required]
        [MaxLength(44)]
        [Column("password")]
        public string Password { get; set; }


        [Required]
        [MaxLength(44)]
        [Column("salt")]
        public string Salt { get; set; }

        [MaxLength(32)]
        [Column("activationkey")]
        public string Activationkey { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(45, ErrorMessage = "Deine E-Mail-Addresse darf maximal 45 Zeichen lang sein.")]
        [RegularExpressionWithOptions(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Deine E-Mail-Adresse ist fehlerhaft.", RegexOptions = RegexOptions.IgnoreCase)]
        public string Email { get; set; }

        [InverseProperty("AffectedUser")]
        public virtual ICollection<PermissionLogEntry> PermissionLogs { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PermissionLogEntry> GivenPermissionLogs { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserLogin> Logins { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserPermission> Permissions { get; set; }
    }
}
