using System;
using System.Collections.Generic;

namespace CacheAspect.Service
{
    public interface IProductService : IDisposable
    {
        ProductDetailDto GetDetails(int id);
        IEnumerable<ProductDetailsDto> GetAll();

        void UpdateDetails(ProductDetailDto productDetailDto);
    }
}
