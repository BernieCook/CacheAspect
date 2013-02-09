using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CacheAspect.Service;
using CacheAspect.Web.Models.SupplierModel;

namespace CacheAspect.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        //
        // GET: /supplier

        [HttpGet]
        public ActionResult Index()
        {
            var supplierDetailsDtos = _supplierService.GetAll();

            Mapper.CreateMap<SupplierDetailsDto, IndexModel>();
            var indexModels = Mapper.Map<IEnumerable<SupplierDetailsDto>, IEnumerable<IndexModel>>(supplierDetailsDtos);

            return View(indexModels);
        }

        // GET: /supplier/detail/117

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var supplierDetailDto = _supplierService.GetDetails(id);

            Mapper.CreateMap<SupplierDetailDto, DetailModel>();
            var detailModel = Mapper.Map<SupplierDetailDto, DetailModel>(supplierDetailDto);

            return View(detailModel);
        }

        // GET: /supplier/detail/117

        [HttpPost]
        public ActionResult Detail(DetailModel detailModel)
        {
            Mapper.CreateMap<DetailModel, SupplierDetailDto>();
            var supplierDetailDto = Mapper.Map<DetailModel, SupplierDetailDto>(detailModel);

            _supplierService.UpdateDetails(supplierDetailDto);
                
            return RedirectToAction("success");
        }

        // GET: /supplier/success

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
    }
}
