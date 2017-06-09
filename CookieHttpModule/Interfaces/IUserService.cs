namespace CodeSnippets.CookieHttpModule
{
    public interface IUserService
    {
        string GetCurrentUserId();
        bool IsLoggedIn();
    }
}