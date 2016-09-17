using System.Web.Mvc;

namespace uTest.WEB.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}