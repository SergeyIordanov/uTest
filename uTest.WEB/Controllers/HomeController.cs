using System.Collections.Generic;
using System.Web.Mvc;
using uTest.BLL.Infrastructure;
using uTest.BLL.Interfaces;
using uTest.WEB.ViewModels;
using MapperConfig = uTest.WEB.Util.MapperConfig;

namespace uTest.WEB.Controllers
{
    public class HomeController : Controller
    {
        readonly ITestService _testService;
        public HomeController(ITestService serv)
        {
            _testService = serv;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<IEnumerable<TestViewModel>>(_testService.GetTests()));
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            try
            {
                _testService.DeleteTest(id);
            }
            catch (ValidationException)
            {
            }
            return RedirectToAction("Index");
        }
    }
}