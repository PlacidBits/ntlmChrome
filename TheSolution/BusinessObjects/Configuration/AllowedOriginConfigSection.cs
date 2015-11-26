using System.Configuration;

namespace BusinessObjects.Configuration
{
    public class AllowedOriginConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public AllowedOriginsInstanceCollection Instances
        {
            get
            {
                return (AllowedOriginsInstanceCollection)this[""];
            }
            set
            {
                this[""] = value;
            }
        }
    }

    public class AllowedOriginsInstanceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AllowedOriginsInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AllowedOriginsInstanceElement)element).Domain;
        }
    }

    public class AllowedOriginsInstanceElement : ConfigurationElement
    {
        [ConfigurationProperty("domain", IsKey = false, IsRequired = true)]
        public string Domain
        {
            get
            {
                return (string)base["domain"];
            }
            set
            {
                base["domain"] = value;
            }
        }
    }
}
