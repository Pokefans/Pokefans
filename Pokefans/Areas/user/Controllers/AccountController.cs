﻿// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Pokefans.Data;
using Pokefans.Models;
using Pokefans.Security;
using Pokefans.Security.phpBB;
using Pokefans.Util;
using Pokefans.Data.UserData;
using Pokefans.Areas.user.Models;

namespace Pokefans.Areas.user.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;
        private Entities _entities;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager, Entities ents)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _authenticationManager = authenticationManager;
            _entities = ents;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager;
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Areas/user/Views/Account/Login.cshtml");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/user/Views/Account/Login.cshtml", model);
            }

            // Anmeldefehler werden bezüglich einer Kontosperre nicht gezählt.
            // Wenn Sie aktivieren möchten, dass Kennwortfehler eine Sperre auslösen, ändern Sie in "shouldLockout: true".

            SignInStatus result = SignInStatus.Failure;

            // Temporary Solution: Resolve User here. But we want to resolve the email in the Manager class, just like it works for the username
            // see https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.Owin/SignInManager.cs
            User u = _entities.Users.Where(g => g.Email == model.Email).FirstOrDefault();
            // if email not found, try the username
            if(u == null)
                u = _entities.Users.Where(x => x.UserName == model.Email).FirstOrDefault();

            // if the user is still null, that's it
            if (u == null)
            {
                ModelState.AddModelError("", "Benutzername, E-Mail-Adresse oder Passwort falsch.");
                return View("~/Areas/user/Views/Account/Login.cshtml", model);
            }

            // Do we have an old phpBB password?
            if(!string.IsNullOrEmpty(u.phpBBPassword))
            {
                // check if this string matches
                var provider = new phpBBCryptoServiceProvider();

                if(provider.phpbbCheckHash(model.Password, u.phpBBPassword))
                {
                    var hasher = new Pbkdf2PasswordHasher();

                    // password is valid, let's set up the new system & delete the old password.
                    u.Password = hasher.HashPassword(model.Password);
                    u.phpBBPassword = null;

                    _entities.SetModified(u);
                    _entities.SaveChanges();
                }
            }

            // Construct a Login record
            UserLogin l = new UserLogin
            {
                Ip = SecurityUtils.GetIPAddressAsString(HttpContext),
                Time = DateTime.Now,
                UserId = u.Id
            };

            if (!u.EmailConfirmed)
            {
                ModelState.AddModelError("", "Deine E-Mail-Adresse wurde noch nicht bestätigt, deshalb kannst du dich noch nicht anmelden. Klicke auf den Link, den du per E-Mail erhalten hast. Falls du die E-Mail nicht finden solltest, kannst du dir unten einen neuen Aktivierungslink zuschicken.");
                l.Success = false;
                l.Reason = "not activated";

                _entities.UserLogins.Add(l);
                _entities.SaveChanges();
            }
            else
            {
                result = SignInManager.PasswordSignIn(u.UserName, model.Password, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        // in principle, this user is logged in, however, we have to check for bans first
                        var ban = _entities.UserBanlist.FirstOrDefault(x => x.UserId == u.Id && x.IsBanned);
                        if (ban != null)
                        {
                            if(ban.IsBanned && ban.ExpiresOn != null && ban.ExpiresOn.Value > DateTime.Now)
                            {
                                // kill the auth token from the response
                                HttpContext.Response.Cookies.Remove(ConfigurationManager.AppSettings["CookieName"]);

                                // sign out
                                AuthenticationManager.SignOut();
                                ban.User = u;
                                l.Success = false;
                                l.Reason = "banned";
                                _entities.UserLogins.Add(l);
                                _entities.SaveChanges();

                                return View("~/Areas/user/Views/Account/Banned.cshtml", ban);
                            }
                            else
                            {
                                // unbanned, delete the row
                                _entities.UserBanlist.Remove(ban);
                            }
                        }

                        l.Success = true;
                        l.Reason = "";

                        _entities.UserLogins.Add(l);
                        _entities.SaveChanges();
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        // 2FA, not handled currently
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:

                        l.Success = false;
                        l.Reason = "wrong-password";

                        _entities.UserLogins.Add(l);
                        _entities.SaveChanges();

                        ModelState.AddModelError("", "Benutzername, E-Mail-Adresse oder Passwort falsch.");
                        break;
                }
            }
            return View("~/Areas/user/Views/Account/Login.cshtml", model);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public ActionResult VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Verlangen, dass sich der Benutzer bereits mit seinem Benutzernamen/Kennwort oder einer externen Anmeldung angemeldet hat.
            if (!SignInManager.HasBeenVerified())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/user/Views/Account/VerifyCode.cshtml",model);
            }

            // Der folgende Code schützt vor Brute-Force-Angriffen der zweistufigen Codes. 
            // Wenn ein Benutzer in einem angegebenen Zeitraum falsche Codes eingibt, wird das Benutzerkonto 
            // für einen bestimmten Zeitraum gesperrt. 
            // Sie können die Einstellungen für Kontosperren in "IdentityConfig" konfigurieren.
            var result = SignInManager.TwoFactorSignIn(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Ungültiger Code.");
                    return View("~/Areas/user/Views/Account/VerifyCode.cshtml", model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("~/Areas/user/Views/Account/Register.cshtml");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Registered = DateTime.Now,
                    RegisteredIp = SecurityUtils.GetIPAddressAsString(HttpContext),
                    EmailConfirmed = false, // todo: add email validation
                    IsLockedOut = false,
                    GravatarEnabled = true
                };

                user.Url = user.GenerateUrl();

                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    //SignInManager.SignIn(user, isPersistent:false, rememberBrowser:false);
                    UserProfile p = new UserProfile();
                    UserFeedConfig ufc = new UserFeedConfig();
                    p.UserId = user.Id;
                    ufc.UserId = user.Id;
                    _entities.UserProfile.Add(p);
                    _entities.UserFeedConfigs.Add(ufc);
                    _entities.SaveChanges();
                    // Weitere Informationen zum Aktivieren der Kontobestätigung und Kennwortzurücksetzung finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=320771".
                    // E-Mail-Nachricht mit diesem Link senden
                    string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    UserManager.SendEmail(user.Id, "[Pokefans] E-Mail bestätigen", this.RenderViewToString("~/Areas/user/Mails/Confirm.cshtml", new ConfirmationMailViewModel() { User = user, CallbackUrl = callbackUrl, ConfirmationKey = code}));

                    return Redirect(Url.Map("/", null)); // TODO: confirmation page
                }
                AddErrors(result);
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            return View("~/Areas/user/Views/Account/Register.cshtml", model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public ActionResult ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var result = UserManager.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "~/Areas/user/Views/Account/ConfirmEmail.cshtml" : "~/Areas/user/Views/Account/Error");
        }

        [AllowAnonymous]
        public ActionResult ResendMailVerification() 
        {
            return View("~/Areas/user/Views/Account/ResendMailVerification.cshtml");   
        }

        [AllowAnonymous]
        [ActionName("ResendMailVerification")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResendMailVerificationConfirm(string mail) {
            User u = UserManager.FindByEmail(mail);

            if (u != null && u.EmailConfirmed == false)
            {
                string code = UserManager.GenerateEmailConfirmationToken(u.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = u.Id, code = code }, protocol: Request.Url.Scheme);
                UserManager.SendEmail(u.Id, "[Pokefans] E-Mail bestätigen", this.RenderViewToString("~/Areas/user/Mails/Confirm.cshtml", new ConfirmationMailViewModel() { User = u, CallbackUrl = callbackUrl, ConfirmationKey = code }));
            }

            return View("~/Areas/user/Views/Account/ResendMailVerificationConfirmed.cshtml");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View("~/Areas/user/Views/Account/ForgotPassword.cshtml");
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);
                if (user == null || !(UserManager.IsEmailConfirmed(user.Id)))
                {
                    // Nicht anzeigen, dass der Benutzer nicht vorhanden ist oder nicht bestätigt wurde.
                    return View("~/Areas/user/Views/Account/ForgotPasswordConfirmation.cshtml");
                }

                // Weitere Informationen zum Aktivieren der Kontobestätigung und Kennwortzurücksetzung finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=320771".
                // E-Mail-Nachricht mit diesem Link senden
                string code = UserManager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                UserManager.SendEmail(user.Id, "Kennwort zurücksetzen", this.RenderViewToString("~/Areas/user/Mails/ResetPassword.cshtml", new ConfirmationMailViewModel() { User = user, CallbackUrl = callbackUrl}));
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            return View("~/Areas/user/Views/Account/ForgotPassword.cshtml", model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View("~/Areas/user/Views/Account/ForgotPasswordConfirmation.cshtml");
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View("~/Areas/user/Views/Account/ResetPassword.cshtml");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindByName(model.Email);
            if (user == null)
            {
                // Nicht anzeigen, dass der Benutzer nicht vorhanden ist.
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = UserManager.ResetPassword(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View("~/Areas/user/Views/Account/ResetPassword.cshtml");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View("~/Areas/user/Views/Account/ResetPasswordConfirmation.cshtml");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Umleitung an den externen Anmeldeanbieter anfordern
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public ActionResult SendCode(string returnUrl, bool rememberMe)
        {
            var userId = SignInManager.GetVerifiedUserId();

            var userFactors = UserManager.GetValidTwoFactorProviders(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View("~/Areas/user/Views/Account/SendCode.cshtml", new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/user/Views/Account/SendCode.cshtml");
            }

            // Token generieren und senden
            if (!SignInManager.SendTwoFactorCode(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Benutzer mit diesem externen Anmeldeanbieter anmelden, wenn der Benutzer bereits eine Anmeldung besitzt
            var result = SignInManager.ExternalSignIn(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Benutzer auffordern, ein Konto zu erstellen, wenn er kein Konto besitzt
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("~/Areas/user/Views/Account/ExternalLoginConfirmation.cshtml", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Informationen zum Benutzer aus dem externen Anmeldeanbieter abrufen
                var info = AuthenticationManager.GetExternalLoginInfo();
                if (info == null)
                {
                    return View("~/Areas/user/Views/Account/ExternalLoginFailure.cshtml");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = UserManager.Create(user);
                if (result.Succeeded)
                {
                    result = UserManager.AddLogin(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View("~/Areas/user/Views/Account/ExternalLoginConfirmation.cshtml", model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View("~/Areas/user/Views/Account/ExternalLoginFailure.cshtml");
        }



        public ActionResult DsgvoCompliance(string redirect) 
        {
            var dsgvo = _entities.DsgvoComplianceInfos.Where(x => x.EffectiveTime <= DateTime.Now).OrderByDescending(g => g.EffectiveTime).First();
            ViewBag.Redirect = redirect;

            return View("~/Areas/user/Views/Account/DsgvoCompliance.cshtml", dsgvo);    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DsgvoCompliance")]
        public ActionResult DsgvoComplianceSave(string Password, string Redirect) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            User u = _entities.Users.First(x => x.Id == uid);

            Pbkdf2PasswordHasher hasher = new Pbkdf2PasswordHasher();

            if(hasher.VerifyHashedPassword(u.Password, Password) != PasswordVerificationResult.Success) {
                ViewBag.Error = "password";
                ViewBag.Redirect = Redirect;
                var dsgvo = _entities.DsgvoComplianceInfos.Where(x => x.EffectiveTime <= DateTime.Now).OrderByDescending(g => g.EffectiveTime).First();
                return View("~/Areas/user/Views/Account/DsgvoCompliance.cshtml", dsgvo);
            }
            u.LastTermsOfServiceAgreement = DateTime.Now;
            _entities.SetModified(u);
            _entities.SaveChanges();

            return this.Redirect(Redirect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Hilfsprogramme
        // Wird für XSRF-Schutz beim Hinzufügen externer Anmeldungen verwendet
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl != null && Regex.IsMatch(returnUrl, @"http(s)?://[a-z\.]+"+ConfigurationManager.AppSettings["Domain"]))
            {
                return Redirect(returnUrl);
            }
            string blubb = Url.Action("Index", "Home", new RouteValueDictionary(new { area = "" }), 
                HttpContext.Request.IsSecureConnection ? "https" : "http", ConfigurationManager.AppSettings["Domain"]);
            return Redirect(blubb);
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}