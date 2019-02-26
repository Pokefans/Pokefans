// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Pokefans.Data;

namespace Pokefans.Security
{
    
    // Konfigurieren des in dieser Anwendung verwendeten Anwendungsbenutzer-Managers. UserManager wird in ASP.NET Identity definiert und von der Anwendung verwendet.
    public class ApplicationUserManager : UserManager<User, int>
    {
        public static IDataProtectionProvider dataProtectionProvider;
        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
            this.PasswordHasher = new Pbkdf2PasswordHasher();

            // Konfigurieren der Überprüfungslogik für Benutzernamen.
            this.UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfigurieren der Überprüfungslogik für Kennwörter.
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Standardeinstellungen für Benutzersperren konfigurieren
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registrieren von Anbietern für zweistufige Authentifizierung. Diese Anwendung verwendet telefonische und E-Mail-Nachrichten zum Empfangen eines Codes zum Überprüfen des Benutzers.
            // Sie können Ihren eigenen Anbieter erstellen und hier einfügen.

            this.RegisterTwoFactorProvider("E-Mail-Code", new EmailTokenProvider<User, int>
            {
                Subject = "Sicherheitscode",
                BodyFormat = "Ihr Sicherheitscode lautet {0}"
            });
            this.EmailService = new PokefansEmailService();

            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
