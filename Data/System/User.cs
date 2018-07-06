// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Pokefans.Data.Attributes;

namespace Pokefans.Data
{
    [Table("system_users")]
    public partial class User : IUser<int>
    {
        public User()
        {
            GravatarOptions = "";
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Du musst einen Benutzernamen angeben")]
        [MaxLength(45, ErrorMessage = "Dein Benutzername darf maximal 45 Zeichen lang sein.")]
        [RegularExpression(@"^[a-zA-ZäöüßÄÖÜ][a-zA-Z0-9-_ äöüßÄÖÜ]{0,43}[a-zA-Z0-9äöüßÄÖÜ]$", ErrorMessage = "Dein Benutzername darf nur aus Großbuchstaben, Kleinbuchstaben, Bindestrich (-) und Unterstich(_) bestehen. Außerdem muss er mit einem Buchstaben beginnen und darf nicht mit Bindestrich oder Unterstrich aufhören.")]
        [Column("name", TypeName = "VARCHAR")]
        [Index]
        public virtual string UserName { get; set; }

        [Required]
        [Column("registered")]
        public virtual DateTime Registered { get; set; }

        [Required]
        [MaxLength(39)]
        [Column("registered_ip")]
        public virtual string RegisteredIp { get; set; }

        [Required]
        [MaxLength(45)]
        [Column("url")]
        [Index]
        public virtual string Url { get; set; }

        [Required]
        [Column("email_confirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [Column("two_factor_enabled")]
        public virtual bool TwoFactorEnabled { get; set; }

        [MaxLength(45)]
        [Column("rank")]
        public virtual string Rank { get; set; }

        [MaxLength(9)]
        [Column("color")]
        public virtual string Color { get; set; }

        [NotMapped]
        public string DisplayCss {
            get {
                if (String.IsNullOrEmpty(Color)) {
                    return "";
                }
                return String.Format("font-weight: bold; color: {0};", Color);
            }
        }

        [Column("unread_notifications")]
        public virtual Nullable<short> UnreadNotificationCount { get; set; }

        [MaxLength(89)]
        [Column("password")]
        public virtual string Password { get; set; }

        [MaxLength(32)]
        [Column("activationkey")]
        public virtual string Activationkey { get; set; }

        [Column("security_stamp")]
        public virtual string SecurityStamp { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(45, ErrorMessage = "Deine E-Mail-Addresse darf maximal 45 Zeichen lang sein.")]
        [RegularExpressionWithOptions(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Deine E-Mail-Adresse ist fehlerhaft.", RegexOptions = RegexOptions.IgnoreCase)]
        public virtual string Email { get; set; }

        [Column("is_locked_out")]
        public virtual bool IsLockedOut { get; set; }

        [Column("locked_out_date")]
        private Nullable<DateTime> lockedOutDate { get; set; }

        [NotMapped]
        public virtual Nullable<DateTimeOffset> LockedOutDate
        {
            get
            {
                return lockedOutDate.HasValue ? new DateTimeOffset(lockedOutDate.Value) : (Nullable<DateTimeOffset>)null;
            }
            set
            {
                lockedOutDate = value.Value.LocalDateTime;
            }
        }

        [Column("access_failed_count")]
        public virtual int AccessFailedCount { get; set; }

        [Column("mini_avatar_filename")]
        protected string _miniAvatarFileName { get; set; }

        public bool GravatarEnabled { get; set; }

        public virtual int FanartCount { get; set; }

        public DateTime? LastTermsOfServiceAgreement { get; set; }

        [NotMapped]
        public virtual string MiniAvatarFileName
        {
            get
            {
                return _miniAvatarFileName ?? "no-avatar.png";
            }
            set
            {
                _miniAvatarFileName = value;
            }
        }

        [NotMapped]
        public string GravatarOptions { get; set; }

        [NotMapped]
        public string AvatarUrl
        {
            get
            {
                if (GravatarEnabled)
                {
                    byte[] bytemail = new UTF8Encoding().GetBytes(Email);
                    byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytemail);
                    return "https://www.gravatar.com/avatar/" + BitConverter.ToString(hash).Replace("-", "").ToLower() + "?d=identicon" + GravatarOptions;
                }
                else
                {
                    return "//files." + ConfigurationManager.AppSettings["Domain"] + "/user/avatare/" + MiniAvatarFileName;
                }
            }
        }

        [NotMapped]
        public virtual Nullable<DateTime> LastLogin
        {
            get
            {
                if (this.Logins.Where(s => s.Success).Count() > 0)
                {
                    return this.Logins.Where(s => s.Success).Max(s => s.Time);
                }
                return null;
            }
        }

        internal static readonly Expression<Func<User, string>> MiniAvatarFileNameExpression = u => u._miniAvatarFileName;

        private ICollection<RoleLogEntry> roleLogs;

        [InverseProperty("AffectedUser")]
        public virtual ICollection<RoleLogEntry> RoleLogs
        {
            get { return roleLogs ?? (roleLogs = new HashSet<RoleLogEntry>()); }
            set { roleLogs = value; }
        }

        private ICollection<RoleLogEntry> givenRoleLogs;

        [InverseProperty("User")]
        public virtual ICollection<RoleLogEntry> GivenRoleLogs
        {
            get { return givenRoleLogs ?? (givenRoleLogs = new HashSet<RoleLogEntry>()); }
            set { givenRoleLogs = value; }
        }

        private ICollection<UserLogin> logins;

        [InverseProperty("User")]
        public virtual ICollection<UserLogin> Logins
        {
            get { return logins ?? (logins = new HashSet<UserLogin>()); }
            set { logins = value; }
        }

        private ICollection<UserRole> roles;

        [InverseProperty("User")]
        public virtual ICollection<UserRole> Roles
        {
            get { return roles ?? (roles = new HashSet<UserRole>()); }
            set { roles = value; }
        }

        private ICollection<UserLoginProvider> providers;

        [InverseProperty("User")]
        public virtual ICollection<UserLoginProvider> LoginProviders
        {
            get { return providers ?? (providers = new HashSet<UserLoginProvider>()); }
            set { providers = value; }
        }

        private ICollection<Content> publishedContents;

        /// <summary>
        /// All contents published by this user
        /// </summary>
        [InverseProperty("PublishedByUser")]
        public virtual ICollection<Content> PublishedContents
        {
            get { return publishedContents ?? (publishedContents = new HashSet<Content>()); }
            set { publishedContents = value; }
        }

        private ICollection<Content> createdContents;

        /// <summary>
        /// All contents published by this user
        /// </summary>
        [InverseProperty("Author")]
        public virtual ICollection<Content> CreatedContents
        {
            get { return createdContents ?? (createdContents = new HashSet<Content>()); }
            set { createdContents = value; }
        }

        private ICollection<ContentVersion> createdContentVersions;
        /// <summary>
        /// All contents published by this user
        /// </summary>
        [InverseProperty("User")]
        public virtual ICollection<ContentVersion> CreatedContentVersions
        {
            get { return createdContentVersions ?? (createdContentVersions = new HashSet<ContentVersion>()); }
            set { createdContentVersions = value; }
        }

        private ICollection<UserNote> notes;

        [InverseProperty("User")]
        public virtual ICollection<UserNote> Notes
        {
            get { return notes ?? (notes = new HashSet<UserNote>()); }
            set { notes = value; }
        }

        private ICollection<UserNote> authorNotes;

        [InverseProperty("Author")]
        public virtual ICollection<UserNote> AuthoredNotes
        {
            get { return authorNotes ?? (authorNotes = new HashSet<UserNote>()); }
            set { authorNotes = value; }
        }

        private ICollection<UserAdvertising> userAdvertisings;

        [InverseProperty("AdvertisingFrom")]
        public virtual ICollection<UserAdvertising> UserAdvertisings
        {
            get { return userAdvertisings ?? (userAdvertisings = new HashSet<UserAdvertising>()); }
            set { userAdvertisings = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one 
            // defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
