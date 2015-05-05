// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Data.Linq;
using Pokefans.Data;
using Pokefans.Security.Exceptions;
using Pokefans.Security.Cryptography;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using System.Net.Mail;
using log4net;

namespace Pokefans.Security
{
    public class PokefansMembershipProvider : MembershipProvider
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PokefansMembershipProvider));
        private Entities context;
        private HttpContextBase requestContext;

        #region Properties
        private string _applicationName;
        /// <summary>
        /// Name of the Application that uses this custom membership provider.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }

        /// <summary>
        /// Specifies wether Users are able to reset their password.
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        /// <summary>
        /// Specifies wether Users are able to retrieve their password.
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 8; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }
        #endregion

        public PokefansMembershipProvider()
            : this(new Entities(), new HttpContextWrapper(HttpContext.Current))
        {
        }

        public PokefansMembershipProvider(Entities e, HttpContextBase b)
        {
            this.context = e;
            this.requestContext = b;
        }

        /// <summary>
        /// Verarbeitet eine Anforderung zum Aktualisieren des Kennworts für einen Mitgliedschaftsbenutzer.
        /// </summary>
        /// <param name="email">Der Benutzer, dessen Kennwort aktualisiert werden soll.</param>
        /// <param name="oldPassword">Das aktuelle Kennwort für den angegebenen Benutzer.</param>
        /// <param name="newPassword">Das neue Kennwort für den angegebenen Benutzer.</param>
        /// <returns>
        /// true, wenn das Kennwort erfolgreich aktualisiert wurde, andernfalls false.
        /// </returns>
        /// <exception cref="Pokefans.Security.Exceptions.UserNotFoundException"></exception>
        /// <exception cref="Pokefans.Security.Exceptions.PasswordMismatchException">Old Password doesn't match the one saved in the database</exception>
        /// <exception cref="System.FormatException">New Password doesn't match the password specifications</exception>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {

            User u = context.Users.Where(g => g.Name == username).FirstOrDefault();

            if (u == null)
            {
                throw new UserNotFoundException();
            }

            if (!CheckPassword(u, oldPassword))
            {
                throw new PasswordMismatchException();
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                throw new FormatException("New Password doesn't match the password specifications");
            }

            byte[] salt = new byte[32];
            Random r = new Random();

            r.NextBytes(salt);

            u.Salt = Convert.ToBase64String(salt);

            Pbkdf2 pbkdf2 = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(newPassword), salt);
            u.Password = Convert.ToBase64String(pbkdf2.GetBytes(32));

            context.SetModified(u);

            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Fügt der Datenquelle einen neuen Mitgliedschaftsbenutzer hinzu.
        /// </summary>
        /// <param name="email">Der Benutzername für den neuen Benutzer.</param>
        /// <param name="password">Das Kennwort für den neuen Benutzer.</param>
        /// <param name="email">Die E-Mail-Adresse für den neuen Benutzer.</param>
        /// <param name="passwordQuestion">Die Kennwortfrage für den neuen Benutzer.</param>
        /// <param name="passwordAnswer">Die Kennwortantwort für den neuen Benutzer.</param>
        /// <param name="isApproved">Gibt an, ob der neue Benutzer für die Überprüfung zugelassen ist.</param>
        /// <param name="providerUserKey">Der eindeutige Bezeichner für den Benutzer aus der Mitgliedschaftsdatenquelle.</param>
        /// <param name="status">Ein <see cref="T:System.Web.Security.MembershipCreateStatus" />-Enumerationswert, der angibt, ob der Benutzer erfolgreich erstellt wurde.</param>
        /// <returns>
        /// Ein <see cref="T:System.Web.Security.MembershipUser" />-Objekt, das die Informationen für den neu erstellten Benutzer enthält.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            
            status = MembershipCreateStatus.ProviderError;

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (ConfigurationManager.AppSettings["InvalidUsernames"].Split(';').Contains(username) || !Regex.IsMatch(username, @"^[a-zA-Z][a-zA-Z0-9-_ ]{0,43}[a-zA-Z0-9]$"))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            User user = context.Users.Where(x => x.Name == username).FirstOrDefault();

            if (user != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            // Enter basic user data
            User u = new User();
            u.Name = username;
            u.Email = email;
            u.Registered = DateTime.Now;
            u.RegisteredIp = SecurityUtils.GetIPAddressAsString(requestContext);
            u.Url = u.GenerateUrl();
            u.Status = isApproved ? (byte)UserStatus.Activated : (byte)UserStatus.NotActivated;

            // Generate a random password salt
            byte[] salt = new byte[32];
            Random r = new Random();
            r.NextBytes(salt);

            // ... and use it to pbkdf2' the users' password
            Pbkdf2 pbkdf2 = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(password), salt);
            u.Salt = Convert.ToBase64String(salt);
            u.Password = Convert.ToBase64String(pbkdf2.GetBytes(32));

            // clear the password from the ram, we don't need it anymore
            password = null;

            if ((UserStatus)u.Status == UserStatus.NotActivated)
            {
                // next, generate an activation key
                byte[] activationkey = new byte[32];
                r.NextBytes(activationkey);
                u.Activationkey = Convert.ToBase64String(activationkey);
            }
            // save the whole thing to the database
            context.Users.Add(u);
            context.SaveChanges();

            if ((UserStatus)u.Status == UserStatus.NotActivated)
            {
                try
                {

                    // Set up an SMTP Client to our desired mail server
                    SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPHost"], int.Parse(ConfigurationManager.AppSettings["SMTPPort"]));
                    smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPassword"]);
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    // Set up a message
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["FromEmailName"]);
                    mail.To.Add(new MailAddress(u.Email));
                    mail.IsBodyHtml = false;

                    // and send an email using the template.
                    DynamicViewBag vb = new DynamicViewBag();
                    vb.AddValue("Domain", ConfigurationManager.AppSettings["Domain"]);
                    mail.Body = Engine.Razor.RunCompile(new LoadedTemplateSource(File.ReadAllText(@".\EmailTemplates\RegisterTemplate.cshtml")), "register-email", typeof(User), u, vb);

                    smtpClient.Send(mail);
                }
                catch (Exception e)
                {
                    log.Error("Failed to send registration email", e);
                }
            }

            // Finally, we're done. Grab the user and return
            status = MembershipCreateStatus.Success;
            return GetUser(u.Email, true);

        }

        /// <summary>
        /// Entfernt einen Benutzer aus der Mitgliedschaftsdatenquelle.
        /// </summary>
        /// <param name="email">Die Email des zu löschenden Benutzers.</param>
        /// <param name="deleteAllRelatedData">true, um die benutzerspezifischen Daten aus der Datenbank zu löschen; false, um die benutzerspezifischen Daten in der Datenbank zu belassen.</param>
        /// <returns>
        /// true, wenn der Benutzer erfolgreich gelöscht wurde, andernfalls false.
        /// </returns>
        /// <exception cref="Pokefans.Security.Exceptions.UserNotFoundException"></exception>
        public override bool DeleteUser(string email, bool deleteAllRelatedData)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();

            if (u == null)
            {
                throw new UserNotFoundException();
            }
            context.Users.Remove(u);

            if (deleteAllRelatedData)
            {
                throw new NotImplementedException();
            }

            context.SaveChanges();

            return true;
        }




        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="userIsOnline">if set to <c>true</c> [user is online].</param>
        /// <returns></returns>
        /// <remarks>This is only implemented because some part in the provider pipeline uses it. I have no Idea what and I don't want to examine. Using this in your code will automatically reject any merge request.</remarks>
        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();
            if (u != null)
            {
                MembershipUser muser = new MembershipUser("PokefansMembershipProvider", u.Name, u.id, u.Email, string.Empty, string.Empty, u.Status > 0, u.Status < 0, u.Registered,
                                                          DateTime.Now, DateTime.Now, DateTime.UtcNow, u.BanTime ?? DateTime.MinValue);
                return muser;
            }
            return null;
        }

        /// <summary>
        /// Ruft Benutzerinformationen aus der Datenquelle auf Grundlage des eindeutigen Bezeichners für den Mitgliedschaftsbenutzer ab. Stellt eine Option zum Aktualisieren des Datums-/Zeitstempels der letzten Aktivität des Benutzers bereit.
        /// </summary>
        /// <param name="providerUserKey">Der eindeutige Bezeichner für den Mitgliedschaftsbenutzer, für den Informationen abgerufen werden sollen.</param>
        /// <param name="userIsOnline">true, um den Datums-/Zeitstempel der letzten Aktivität des Benutzers zu aktualisieren; false, um Benutzerinformationen ohne Aktualisierung des Datums-/Zeitstempels der letzten Aktivität des Benutzers zurückzugeben.</param>
        /// <returns>
        /// Ein mit den Informationen des angegebenen Benutzers aus der Datenquelle aufgefülltes <see cref="T:System.Web.Security.MembershipUser" />-Objekt.
        /// </returns>
        /// <remarks>This is only implemented because some part in the provider pipeline uses it. I have no Idea what and I don't want to examine. Using this in your code will automatically reject any merge request.</remarks>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            User u = context.Users.Where(x => x.id == (int)providerUserKey).FirstOrDefault();
            if (u != null)
            {
                MembershipUser muser = new MembershipUser("PokefansMembershipProvider", u.Name, u.id, u.Email, string.Empty, string.Empty, u.Status > 0, u.Status < 0, u.Registered,
                                                          DateTime.Now, DateTime.Now, DateTime.UtcNow, u.BanTime ?? DateTime.MinValue);
                return muser;
            }
            return null;
        }

        /// <summary>
        /// Setzt das Kennwort eines Benutzers auf ein neues, automatisch generiertes Kennwort zurück.
        /// </summary>
        /// <param name="email">Der Benutzer, dessen Kennwort zurückgesetzt werden soll.</param>
        /// <param name="answer">Die Kennwortantwort für den angegebenen Benutzer.</param>
        /// <returns>
        /// Das neue Kennwort für den angegebenen Benutzer.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string ResetPassword(string email, string answer)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();

            if (u == null)
            {
                throw new UserNotFoundException();
            }

            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            StringBuilder b = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < 11; i++)
                b.Append(chars[r.Next(chars.Length)]);

            Pbkdf2 pbkdf2 = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(b.ToString()), Convert.FromBase64String(u.Salt));
            u.Password = Convert.ToBase64String(pbkdf2.GetBytes(32));

            context.SetModified(u);
            context.SaveChanges();

            return b.ToString();
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="activationkey">The activationkey.</param>
        /// <exception cref="Pokefans.Security.Exceptions.UserNotFoundException">Occurs if the specified User could not be found</exception>
        /// <returns></returns>
        public bool ActivateUser(string email, string activationkey)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();

            if (u == null)
            {
                throw new UserNotFoundException();
            }

            if (u.Activationkey == activationkey)
            {
                u.Activationkey = "";
                u.Status = (byte)UserStatus.Activated;
                context.SetModified(u);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Überprüft, ob die angegebene Email und das Kennwort in der Datenquelle vorhanden sind.
        /// </summary>
        /// <param name="email">Die Email des zu überprüfenden Benutzers.</param>
        /// <param name="password">Das Kennwort für den angegebenen Benutzer.</param>
        /// <returns>
        /// true, wenn die angegebene Email und das angegebene Kennwort gültig sind, andernfalls false.
        /// </returns>
        public override bool ValidateUser(string email, string password)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();

            if (u == null)
            {
                return false;
            }

            if (CheckPassword(u, password))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the password against the given User's password
        /// </summary>
        /// <param name="u">The User.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private bool CheckPassword(User u, string password)
        {
            Pbkdf2 pbkdf = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(password), Convert.FromBase64String(u.Salt));

            byte[] result = pbkdf.GetBytes(32);
            byte[] data = Convert.FromBase64String(u.Password);

            return result.SequenceEqual(data);
        }

        #region useless stubs

        /********************************************************************
         * The following methods are here because they have to be. However,
         * they will not be used (neither by us nor by .net itself). It is 
         * therefore acceptable to just throw an NotImplementedException.
         *******************************************************************/
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            User u = context.Users.Where(x => x.Email == email).FirstOrDefault();
            return u != null ? u.Name : null;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
