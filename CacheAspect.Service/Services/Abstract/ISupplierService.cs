using System;
using System.Collections.Generic;

namespace CacheAspect.Service
{
    public interface ISupplierService : IDisposable
    {
        SupplierDetailDto GetDetails(int id);
        IEnumerable<SupplierDetailsDto> GetAll();

        void UpdateDetails(SupplierDetailDto supplierDetailDto);
    }
}
