using System;
using System.Collections.Generic;

namespace CacheAspect.Service
{
    public interface ICustomerService : IDisposable
    {
        CustomerDetailDto GetDetails(string id);
        IEnumerable<CustomerDetailsDto> GetAllByContactTitle(string contactTitle);

        void UpdateDetails(CustomerDetailDto customerDetailDto);
    }
}
