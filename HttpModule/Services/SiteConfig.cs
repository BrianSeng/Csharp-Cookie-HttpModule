using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Example.Namespace.HttpModule.Services
{
    //key vault for pulling keys from Web.config
    public class SiteConfig : IConfigService
    {
        public string ExampleApiKey
        {
            get
            {
                return GetSetting("Example.ApiKey");
            }
        }

        public string[] KeysToWatchFor
        {
            get
            {
                return GetRcKeys();
            }
        }

        private string GetSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

        private string[] GetRcKeys()
        {
            string[] keys = WebConfigurationManager.AppSettings["WatchedKeys"].Split(',');

            return keys;
        }
    }
}
