using Ninject.Modules;
using uTest.DAL.Interfaces;
using uTest.DAL.Repositories;

namespace uTest.BLL.Infrastructure
{
    /// <summary>
    /// Ninject service for sending dependency injection configuration to presentation layer
    /// </summary>
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;

        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<TestUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
