using System.Web;

namespace Example.Namespace.HttpModule.Interfaces
{
    public interface ICookieService
    {
        void ReadCookies(HttpCookieCollection cookies, string userId, bool isAnon);
    }
}
