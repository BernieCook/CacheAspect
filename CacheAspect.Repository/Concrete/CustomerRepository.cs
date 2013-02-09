using System.Collections.Generic;
using CacheAspect.DomainModel;

namespace CacheAspect.Repository
{
    public class CustomerRepository : GenericRepository<Customer> ,ICustomerRepository
    {
        public CustomerRepository(NorthwindContext context) 
            : base(context)
        {
        }

        [Cache(CacheAction.Add)]
        public IEnumerable<Customer> GetAllByContactTitle(string contactTitle)
        {
            return GetAll(q => q.ContactTitle.Contains(contactTitle));
        }
    }
}