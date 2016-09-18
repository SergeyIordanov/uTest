using System.Web.Mvc;

namespace uTest.WEB.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}