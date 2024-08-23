namespace Webapi.configuration
{
    public class Config
    {
        public class LaunchSettings
        {
            public IisSettings iisSettings { get; set; }
            public Profiles profiles { get; set; }
        }

        public class IisSettings
        {
            public string applicationUrl { get; set; }
            public int sslPort { get; set; }
        }

        public class Profiles
        {
            public Profile http { get; set; }
            public Profile https { get; set; }
            public Profile IISExpress { get; set; }
        }

        public class Profile
        {
            public string applicationUrl { get; set; }
        }

    }
}
