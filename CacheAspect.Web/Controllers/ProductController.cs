using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CacheAspect.Service;
using CacheAspect.Web.Models.ProductModel;

namespace CacheAspect.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //
        // GET: /product

        [HttpGet]
        public ActionResult Index()
        {
            var productDetailsDtos = _productService.GetAll();

            Mapper.CreateMap<ProductDetailsDto, IndexModel>();
            var indexModels = Mapper.Map<IEnumerable<ProductDetailsDto>, IEnumerable<IndexModel>>(productDetailsDtos);

            return View(indexModels);
        }

        // GET: /product/detail/117

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var productDetailDto = _productService.GetDetails(id);

            Mapper.CreateMap<ProductDetailDto, DetailModel>();
            var detailModel = Mapper.Map<ProductDetailDto, DetailModel>(productDetailDto);

            return View(detailModel);
        }

        // GET: /product/detail/117

        [HttpPost]
        public ActionResult Detail(DetailModel detailModel)
        {
            Mapper.CreateMap<DetailModel, ProductDetailDto>();
            var productDetailDto = Mapper.Map<DetailModel, ProductDetailDto>(detailModel);

            _productService.UpdateDetails(productDetailDto);
                
            return RedirectToAction("success");
        }

        // GET: /product/success

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
    }
}
