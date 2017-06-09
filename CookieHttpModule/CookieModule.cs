using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.Practices.Unity;
using System.Net.Http.Headers;
using CodeSnippets.CookieHttpModule;

namespace Sabio.Web.Classes.HttpModule
{
    public class CookieModule : IHttpModule
    {
        private IUserService _userService = null;
        private IConfigService _siteConfig = null;
        private ICookieService _cookiesService = null;

        public void Dispose()
        {
            //nothing
        }

        public void Init(HttpApplication context)
        {

            context.PostRequestHandlerExecute += (new EventHandler(this.On_PostRequestHandlerExecute));

            //resolving dependencies to implement needed Service methods (required Microsoft.Practices.Unity using statement)
            _userService = UnityConfig.GetContainer().Resolve<IUserService>();
            _siteConfig = UnityConfig.GetContainer().Resolve<IConfigService>();
            _cookiesService = UnityConfig.GetContainer().Resolve<ICookieService>();
        }

        private void On_PostRequestHandlerExecute(object requestSource, EventArgs e)
        {
            HttpRequest currentRequest = HttpContext.Current.Request;
            CookieFromQryStrng(currentRequest);

            if (HttpContext.Current.Response.Cookies != null && HttpContext.Current.Response.Cookies.Keys.Count > 0)
            {
                HttpCookieCollection cookies = HttpContext.Current.Response.Cookies;
                string userId = GetUserId(currentRequest);
                _cookiesService.ReadCookies(cookies, userId, true);
            }
            else if (_userService.IsLoggedIn() && HttpContext.Current.Request.Cookies["AnonUser"] != null)
            {
                HttpContext.Current.Response.Cookies["AnonUser"].Value = "";
                HttpContext.Current.Response.Cookies["AnonUser"].Expires = DateTime.Today.AddDays(-1d);
            }
        }

        private void CookieFromQryStrng(HttpRequest currentRequest)
        {
            if (currentRequest.QueryString.Keys != null && currentRequest.QueryString.Keys.Count > 0)
            {
                foreach (var key in _siteConfig.KeysToWatchFor)
                {
                    string qryString = currentRequest.QueryString[key];
                    string[] stringSeparators = new string[] { "," };

                    //if qryString is null/empty, start the loop over again
                    //because we still want to capture empty keys
                    if (string.IsNullOrEmpty(qryString))
                    {
                        continue;
                    }

                    HttpCookie cookie = currentRequest.Cookies[GetCookieName(key)];

                    if (cookie == null)
                    {
                        cookie = new HttpCookie(GetCookieName(key));
                    }

                    //if the key's query string contains multiple subkeys (e.g. "blue,red = timestamp") - split it
                    string[] splitQryString = qryString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var val in splitQryString)
                    {
                        if (val != null)
                        {
                            SetCookie(cookie, val);
                        }
                    }

                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }

        private string GetUserId(HttpRequest currentRequest)
        {
            string userId = null;
            if (_userService.IsLoggedIn())
            {
                userId = _userService.GetCurrentUserId();
                if (currentRequest.Cookies["AnonUser"] != null)
                {
                    HttpContext.Current.Response.Cookies["AnonUser"].Value = "";
                    HttpContext.Current.Response.Cookies["AnonUser"].Expires = DateTime.Today.AddDays(-1d);
                }
            }
            else
            {

                HttpCookie anonIdCookie = null;
                if (currentRequest.Cookies["AnonUser"] != null && currentRequest.Cookies["AnonUser"].Values.AllKeys.Contains("Id"))
                {
                    userId = currentRequest.Cookies["AnonUser"]["Id"];
                    anonIdCookie = currentRequest.Cookies["AnonUser"];
                }
                else if (currentRequest.Cookies["AnonUser"] == null || currentRequest.Cookies["AnonUser"] != null && !currentRequest.Cookies["AnonUser"].Values.AllKeys.Contains("Id"))
                {
                    anonIdCookie = HttpContext.Current.Response.Cookies["AnonUser"];
                    anonIdCookie["Id"] = Guid.NewGuid().ToString();
                    userId = anonIdCookie["Id"];
                }

                HttpContext.Current.Response.Cookies.Add(anonIdCookie);
            }
            return userId;
        }

        private string GetCookieName(string key)
        {
            return "RC_" + key;
        }

        private void SetCookie(HttpCookie cookie, string newVal)
        {
            cookie.Values.Add(newVal, DateTime.UtcNow.ToFileTimeUtc().ToString());
        }

    }
}