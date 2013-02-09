using System.Web.Mvc;

namespace CacheAspect.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
