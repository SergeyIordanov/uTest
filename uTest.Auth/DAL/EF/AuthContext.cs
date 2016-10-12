using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.EF
{
    /// <summary>
    /// Context for working with related database
    /// </summary>
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Static constructor for setting DB initializer
        /// </summary>
        static AuthContext()
        {
            //Database.SetInitializer(new AuthDbInitializer());
        }

        public AuthContext(string conectionString) : base(conectionString)
        {
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
    }
}
