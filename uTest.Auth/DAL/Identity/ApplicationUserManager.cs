using Microsoft.AspNet.Identity;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.Identity
{
    /// <summary>
    /// Identity class for manage users
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }
    }
}