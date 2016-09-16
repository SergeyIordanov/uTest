using uTest.Auth.DAL.EF;
using uTest.Auth.DAL.Interfaces;
using uTest.Entities.Identity;

namespace uTest.Auth.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public AuthContext Database { get; set; }

        public ClientManager(AuthContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}