using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data.Entity.Validation;
using System.Data;

namespace Example.Namespace.HttpModule.Services
{
    public class UserService : IUserService
    {
        public string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetCurrentUserId());
        }
    }
}
