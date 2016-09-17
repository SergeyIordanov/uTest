using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
            //TEST
            //_testService.CreateTask(new TaskDTO {MinResult = 60, UserId = User.Identity.GetUserId()},  1);

            //_testService.SolveTask(_testService.GetTasks(User.Identity.GetUserId()).Last(), 40);
            //----

            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            if(User.Identity.IsAuthenticated)
                return View(mapper.Map<StatisticViewModel>(_testService.GetStatistic(User.Identity.GetUserId())));
            return View(model: null);
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