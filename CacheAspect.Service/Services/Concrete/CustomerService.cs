using System.Collections.Generic;
using System.Linq;
using CacheAspect.DomainModel;
using CacheAspect.Repository;

namespace CacheAspect.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            IUnitOfWork unitOfWork, 
            ICustomerRepository customerRepository)
        {
            UnitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public CustomerDetailDto GetDetails(
            string id)
        {
            var customer = _customerRepository.GetById(id);

            return new CustomerDetailDto
                            {
                                CustomerId = customer.CustomerID,
                                CompanyName = customer.CompanyName,
                                ContactName = customer.ContactName,
                                ContactTitle = customer.ContactTitle
                            };
        }

        public IEnumerable<CustomerDetailsDto> GetAllByContactTitle(
            string contactTitle)
        {
            return _customerRepository.GetAllByContactTitle(contactTitle)
                                     .Select(c =>
                                             new CustomerDetailsDto
                                                 {
                                                     CustomerId = c.CustomerID,
                                                     CompanyName = c.CompanyName,
                                                     ContactName = c.ContactName,
                                                     ContactTitle = c.ContactTitle
                                                 });
        }

        public void UpdateDetails(
            CustomerDetailDto customerDetailDto)
        {
            var customer = _customerRepository.GetById(customerDetailDto.CustomerId);

            customer.CompanyName = customerDetailDto.CompanyName;
            customer.ContactName = customerDetailDto.ContactName;
            customer.ContactTitle = customerDetailDto.ContactTitle;

            _customerRepository.Update(customer);

            UnitOfWork.Commit();
        }
    }
}
