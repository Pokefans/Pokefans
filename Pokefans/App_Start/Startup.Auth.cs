// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Pokefans.Data;
using Pokefans.Security;

namespace Pokefans
{
    public partial class Startup
    {
        // Weitere Informationen zum Konfigurieren der Authentifizierung finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=301864".
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

            // Anwendung für die Verwendung eines Cookies zum Speichern von Informationen für den angemeldeten Benutzer aktivieren
            // und ein Cookie zum vorübergehenden Speichern von Informationen zu einem Benutzer zu verwenden, der sich mit dem Anmeldeanbieter eines Drittanbieters anmeldet.
            // Konfigurieren des Anmeldecookies.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/anmeldung"),
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = ApplyRedirect,
                    // Aktiviert die Anwendung für die Überprüfung des Sicherheitsstempels, wenn sich der Benutzer anmeldet.
                    // Dies ist eine Sicherheitsfunktion, die verwendet wird, wenn Sie ein Kennwort ändern oder Ihrem Konto eine externe Anmeldung hinzufügen.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => { return user.GenerateUserIdentityAsync(manager); },
                        getUserIdCallback: (id) => { return id.GetUserId<int>(); })
                },
                CookieDomain = ConfigurationManager.AppSettings["CookieDomain"],
                CookieName = ConfigurationManager.AppSettings["CookieName"]
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Aktiviert die Anwendung für das vorübergehende Speichern von Benutzerinformationen beim Überprüfen der zweiten Stufe im zweistufigen Authentifizierungsvorgang.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Aktiviert die Anwendung für das Speichern der zweiten Anmeldeüberprüfungsstufe (z. B. Telefon oder E-Mail).
            // Wenn Sie diese Option aktivieren, wird Ihr zweiter Überprüfungsschritt während des Anmeldevorgangs auf dem Gerät gespeichert, von dem aus Sie sich angemeldet haben.
            // Dies ähnelt der RememberMe-Option bei der Anmeldung.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Auskommentierung der folgenden Zeilen aufheben, um die Anmeldung mit Anmeldeanbietern von Drittanbietern zu ermöglichen
            if (bool.Parse(ConfigurationManager.AppSettings["UseMicrosoftAuthentication"]))
            {
                app.UseMicrosoftAccountAuthentication(
                    clientId: ConfigurationManager.AppSettings["MicrosoftAuthenticationClientId"],
                    clientSecret: ConfigurationManager.AppSettings["MicrosoftAuthenticationClientSecret"]);
            }

            if (bool.Parse(ConfigurationManager.AppSettings["UseTwitterAuthentication"]))
            {
                app.UseTwitterAuthentication(
                   consumerKey: ConfigurationManager.AppSettings["TwitterAuthenticationConsumerKey"],
                   consumerSecret: ConfigurationManager.AppSettings["TwitterAuthenticationConsumerSecret"]);
            }

            if (bool.Parse(ConfigurationManager.AppSettings["UseFacebookAuthentication"]))
            {
                app.UseFacebookAuthentication(
                   appId: ConfigurationManager.AppSettings["FacebookAuthenticationAppId"],
                   appSecret: ConfigurationManager.AppSettings["FacebookAuthenticationAppSecret"]);
            }

            if (bool.Parse(ConfigurationManager.AppSettings["UseGoogleAuthentication"]))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = ConfigurationManager.AppSettings["GoogleAuthenticationClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["GoogleAuthenticationClientSecret"]
                });
            }
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            Uri absoluteUri;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absoluteUri))
            {
                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                {
                    if (bool.Parse(ConfigurationManager.AppSettings["HttpsLoginPage"]))
                        context.RedirectUri = "https://";
                    else
                        context.RedirectUri = "http://";
                    context.RedirectUri += "user." + ConfigurationManager.AppSettings["Domain"] + ":" +
                                           ConfigurationManager.AppSettings["port"] + "/anmeldung";
                    context.RedirectUri += new QueryString(
                        context.Options.ReturnUrlParameter,
                        context.Request.Uri.AbsoluteUri).ToString();
                }
            }

            context.Response.Redirect(context.RedirectUri);
        }
    }
}