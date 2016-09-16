using uTest.Auth.BLL.Interfaces;
using uTest.Auth.DAL.Repositories;

namespace uTest.Auth.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            // Here you can change IUnitOfWork implementation & IUserService implementation
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}
