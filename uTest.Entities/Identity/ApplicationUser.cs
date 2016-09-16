using Microsoft.AspNet.Identity.EntityFramework;

namespace uTest.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Navigation property for (related to user) client profile
        /// </summary>
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
