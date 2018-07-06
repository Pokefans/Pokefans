using System;
using System.Configuration;

namespace Pokefans.Security
{
    public class MailSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("server")]
        public ServerElement Server
        {
            get { return (ServerElement)this["server"]; }
            set { this["server"] = value; }
        }

        [ConfigurationProperty("credentials")]
        public CredentialsElement Credentials 
        {
            get { return (CredentialsElement)this["credentials"]; }
            set { this["credentials"] = value; }
        }

        [ConfigurationProperty("identity")]
        public IdentityElement Identity 
        {
            get { return (IdentityElement)this["identity"]; }
            set { this["identity"] = value; }
        }
    }

    public class ServerElement : ConfigurationElement 
    {
        [ConfigurationProperty("host", DefaultValue = "localhost", IsRequired = false)]
        public String Host 
        {
            get { return (String)this["host"]; }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "25", IsRequired = false)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("trustAllSSL", DefaultValue = true, IsRequired = false)]
        public bool TrustAllSsl
        {
            get { return (bool)this["trustAllSSL"]; }
            set { this["trustAllSSL"] = value; }
        }
    }

    public class CredentialsElement : ConfigurationElement
    {
        [ConfigurationProperty("user", DefaultValue = "", IsRequired = false)]
        public String User
        {
            get { return (String)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsRequired = false)]
        public String Password
        {
            get { return (String)this["password"]; }
            set { this["password"] = value; }
        }
    }

    public class IdentityElement : ConfigurationElement 
    {
        [ConfigurationProperty("from", IsRequired = true)]
        public String From
        {
            get { return (String)this["from"]; }
            set { this["from"] = value; }
        }

        [ConfigurationProperty("replyto", DefaultValue = "", IsRequired = false)]
        public String ReplyTo
        {
            get { return (String)this["replyto"]; }
            set { this["replyto"] = value; }
        }
    }
}
