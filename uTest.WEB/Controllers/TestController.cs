using System.Collections.Generic;
using System.Web.Mvc;
using uTest.BLL.Interfaces;
using uTest.WEB.Util;
using uTest.WEB.ViewModels;

namespace uTest.WEB.Controllers
{
    public class TestController : Controller
    {
        readonly ITestService _testService;
        public TestController(ITestService serv)
        {
            _testService = serv;
        }

        [HttpGet]
        public ActionResult Public()
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<IEnumerable<TestViewModel>>(_testService.GetPublicTests()));
        }
    }
}