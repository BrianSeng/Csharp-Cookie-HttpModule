using System.Web;

namespace CodeSnippets.CookieHttpModule
{
    public interface ICookieService
    {
        void ReadCookies(HttpCookieCollection cookies, string userId, bool isAnon);
    }
}