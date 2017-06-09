using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CodeSnippets.CookieHttpModule.Services
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
            string[] RefCodes = WebConfigurationManager.AppSettings["WatchedKeys"].Split(',');

            return RefCodes;
        }
    }
}