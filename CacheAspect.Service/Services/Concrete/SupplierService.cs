using System.Collections.Generic;
using System.Linq;
using CacheAspect.DomainModel;
using CacheAspect.Repository;

namespace CacheAspect.Service
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public SupplierService(
            IUnitOfWork unitOfWork, 
            IGenericRepository<Supplier> supplierRepository)
        {
            UnitOfWork = unitOfWork;
            _supplierRepository = supplierRepository;
        }

        public SupplierDetailDto GetDetails(
            int id)
        {
            var supplier = _supplierRepository.GetById(id);

            return new SupplierDetailDto
                            {
                                SupplierId = supplier.SupplierID,
                                CompanyName = supplier.CompanyName,
                                ContactName = supplier.ContactName,
                                ContactTitle = supplier.ContactTitle
                            };
        }

        public IEnumerable<SupplierDetailsDto> GetAll()
        {
            return _supplierRepository.GetAll()
                                     .Select(s =>
                                             new SupplierDetailsDto
                                                 {
                                                     SupplierId = s.SupplierID,
                                                     CompanyName = s.CompanyName,
                                                     City = s.City
                                                 });
        }

        public void UpdateDetails(
            SupplierDetailDto supplierDetailDto)
        {
            var supplier = _supplierRepository.GetById(supplierDetailDto.SupplierId);

            supplier.CompanyName = supplierDetailDto.CompanyName;
            supplier.ContactName = supplierDetailDto.ContactName;
            supplier.ContactTitle = supplierDetailDto.ContactTitle;

            _supplierRepository.Update(supplier);

            UnitOfWork.Commit();
        }
    }
}
