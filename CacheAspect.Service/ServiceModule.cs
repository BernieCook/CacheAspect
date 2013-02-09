using CacheAspect.Repository;
using CacheAspect.DomainModel;
using Ninject.Modules;

namespace CacheAspect.Service
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICustomerRepository>().To<CustomerRepository>();
            Bind<IGenericRepository<Product>>().To<GenericRepository<Product>>();
            Bind<IGenericRepository<Supplier>>().To<GenericRepository<Supplier>>();
        }
    }
}
