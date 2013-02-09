using System.Collections.Generic;
using System.Linq;
using CacheAspect.DomainModel;
using CacheAspect.Repository;

namespace CacheAspect.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(
            IUnitOfWork unitOfWork, 
            IGenericRepository<Product> productRepository)
        {
            UnitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public ProductDetailDto GetDetails(
            int id)
        {
            var product = _productRepository.GetById(id);

            return new ProductDetailDto
                            {
                                ProductId = product.ProductID,
                                ProductName = product.ProductName,
                                SupplierId = product.SupplierID,
                                UnitPrice = product.UnitPrice,
                                UnitsInStock = product.UnitsInStock,
                                UnitsOnOrder = product.UnitsOnOrder
                            };
        }

        public IEnumerable<ProductDetailsDto> GetAll()
        {
            return _productRepository.GetAll()
                                     .Select(p =>
                                             new ProductDetailsDto
                                                 {
                                                     Id = p.ProductID,
                                                     Name = p.ProductName,
                                                     UnitPrice = p.UnitPrice
                                                 });
        }

        public void UpdateDetails(
            ProductDetailDto productDetailDto)
        {
            var product = _productRepository.GetById(productDetailDto.ProductId);

            product.ProductName = productDetailDto.ProductName;
            product.UnitPrice = productDetailDto.UnitPrice;
            product.UnitsInStock = productDetailDto.UnitsInStock;
            product.UnitsOnOrder = productDetailDto.UnitsOnOrder;

            _productRepository.Update(product);

            UnitOfWork.Commit();
        }
    }
}
