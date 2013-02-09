using System.Collections.Generic;
using CacheAspect.DomainModel;

namespace CacheAspect.Repository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IEnumerable<Customer> GetAllByContactTitle(string contactTitle);
    }
}
