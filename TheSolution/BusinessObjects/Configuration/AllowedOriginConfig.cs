using System.Collections.Generic;

namespace BusinessObjects.Configuration
{
    public class AllowedOriginConfig
    {
        protected static Dictionary<string, AllowedOriginsInstanceElement> _instances;

        protected static List<string> _domains;

        static AllowedOriginConfig()
        {
            _instances = new Dictionary<string, AllowedOriginsInstanceElement>();
            _domains = new List<string>();
            var sec = (AllowedOriginConfigSection)System.Configuration.ConfigurationManager.GetSection("allowedOriginsConfig");
            foreach (AllowedOriginsInstanceElement i in sec.Instances)
            {
                _instances.Add(i.Domain, i);
                _domains.Add(i.Domain);
            }
        }
        public static AllowedOriginsInstanceElement Instances(string instanceName)
        {
            return _instances[instanceName];
        }

        public static List<string> GetDomains()
        {
            return _domains;
        }

        private AllowedOriginConfig()
        {
        }
    }
}
