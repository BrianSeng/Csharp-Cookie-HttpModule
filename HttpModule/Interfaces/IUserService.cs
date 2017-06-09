namespace Exmaple.Namespace.HttpModule.Interfaces
{
    public interface IUserService
    {
        string GetCurrentUserId();
        bool IsLoggedIn();
    }
}
