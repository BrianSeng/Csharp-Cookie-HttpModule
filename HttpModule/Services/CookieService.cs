using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Example.Namespace.HttpModule.Services
{
    public class CookieService : ICookieService
    {
        private IConfigService _siteConfig = null;
        public CookieService(IConfigService configService)
        {
            _siteConfig = configService;
        }

        public void ReadCookies(HttpCookieCollection cookies, string userId, bool isAnon)
        {
            foreach (var key in _siteConfig.KeysToWatchFor)
            {
                string RC_Key = "RC_" + key;
                HttpCookie cookie = cookies.Get(RC_Key);
                if (cookie != null)
                {
                    NameValueCollection nameValPair = cookie.Values;
                    string[] allVals = nameValPair.AllKeys;

                    foreach (var val in allVals)
                    {
                        if (val != null && !CheckValExists(userId, key, val))
                        {
                            string[] tStamps = nameValPair.GetValues(val);

                            foreach (var t in tStamps)
                            {
                                if (t != "" && t != null)
                                {
                                    InsertReferralCode(userId, key, val, DateTime.FromFileTimeUtc(Convert.ToInt64(t)), isAnon);
                                }

                            }
                        }
                    }
                }
            }
        }

        public bool CheckValExists(string userId, string key, string value)
        {
            //Check if value exists in your DB
            //via your stored SQL procedure
        }

        public void InsertCookieVals(string userId, string keywordCode, string value, DateTime timeStamp, bool isAnon)
        {
            //Insert cookie vals into DB
            //via your stored SQL procedure
        }
    }
}
