using System.Collections.Generic;
using System.Web.Mvc;
using uTest.BLL.Interfaces;
using uTest.WEB.Util;
using uTest.WEB.ViewModels;

namespace uTest.WEB.Controllers
{
    public class HomeController : Controller
    {
        readonly ITestService _rentService;
        public HomeController(ITestService serv)
        {
            _rentService = serv;
        }

        public ActionResult Index()
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<IEnumerable<TestViewModel>>(_rentService.GetTests()));
        }
    }
}