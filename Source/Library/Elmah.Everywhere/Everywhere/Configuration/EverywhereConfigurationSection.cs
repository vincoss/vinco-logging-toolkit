using System.Configuration;


namespace Elmah.Everywhere.Configuration
{
    public class EverywhereConfigurationSection : ConfigurationSection
    {
        public static string SectionKey = "everywhere/settings";

        static EverywhereConfigurationSection()
        {
        }

        [ConfigurationProperty("remoteLogUri", IsRequired = true)]
        public string RemoteLogUri
        {
            get { return ((string)base["remoteLogUri"]); }
            set { base["remoteLogUri"] = value; }
        }

        [ConfigurationProperty("token", IsRequired = true)]
        public string Token
        {
            get { return ((string)base["token"]); }
            set { base["token"] = value; }
        }

        [ConfigurationProperty("applicationName", IsRequired = true)]
        public string ApplicationName
        {
            get { return ((string)base["applicationName"]); }
            set { base["applicationName"] = value; }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return ((string)base["host"]); }
            set { base["host"] = value; }
        }
    }
}
