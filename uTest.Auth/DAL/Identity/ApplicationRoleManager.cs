using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.Identity
{
    /// <summary>
    /// Identity class for manage roles
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
                    : base(store)
        { }
    }
}
