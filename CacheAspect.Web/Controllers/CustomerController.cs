using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CacheAspect.Service;
using CacheAspect.Web.Models.CustomerModel;

namespace CacheAspect.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //
        // GET: /customer

        [HttpGet]
        public ActionResult Index()
        {
            var customerDetailsDtos = _customerService.GetAllByContactTitle("Sales");

            Mapper.CreateMap<CustomerDetailsDto, IndexModel>();
            var indexModels = Mapper.Map<IEnumerable<CustomerDetailsDto>, IEnumerable<IndexModel>>(customerDetailsDtos);

            return View(indexModels);
        }

        // GET: /customer/detail/117

        [HttpGet]
        public ActionResult Detail(string id)
        {
            var customerDetailDto = _customerService.GetDetails(id);

            Mapper.CreateMap<CustomerDetailDto, DetailModel>();
            var detailModel = Mapper.Map<CustomerDetailDto, DetailModel>(customerDetailDto);

            return View(detailModel);
        }

        // GET: /customer/detail/117

        [HttpPost]
        public ActionResult Detail(DetailModel detailModel)
        {
            Mapper.CreateMap<DetailModel, CustomerDetailDto>();
            var customerDetailDto = Mapper.Map<DetailModel, CustomerDetailDto>(detailModel);

            _customerService.UpdateDetails(customerDetailDto);
                
            return RedirectToAction("success");
        }

        // GET: /customer/success

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
    }
}
