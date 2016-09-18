using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uTest.BLL.DTO;
using uTest.BLL.Infrastructure;
using uTest.BLL.Interfaces;
using uTest.WEB.ViewModels;
using MapperConfig = uTest.WEB.Util.MapperConfig;

namespace uTest.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminTestController : Controller
    {
        readonly ITestService _testService;
        public AdminTestController(ITestService serv)
        {
            _testService = serv;
        }

        [HttpGet]       
        public ActionResult Index()
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            var tests = mapper.Map<IEnumerable<TestViewModel>>(_testService.GetTests());
            foreach (var test in tests)
            {
                test.SolvedTests =
                    mapper.Map<IEnumerable<SolvedTestViewModel>>(_testService.GetSolvedTests(test.Id)).ToList();
            }
            return View(tests);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestViewModel testView)
        {
            try
            {
                var mapper = MapperConfig.GetConfigFromViewModelToDTO().CreateMapper();
                _testService.CreateTest(mapper.Map<TestDTO>(testView));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(testView);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            try
            {
                var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
                return View(mapper.Map<TestViewModel>(_testService.GetTest(id)));
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestViewModel testView)
        {
            try
            {              
                var mapper = MapperConfig.GetConfigFromViewModelToDTO().CreateMapper();
                _testService.UpdateTest(mapper.Map<TestDTO>(testView));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(testView);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                _testService.DeleteTest(id);
            }
            catch (ValidationException)
            {}

            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return PartialView("Partials/_TestsList", mapper.Map<IEnumerable<TestViewModel>>(_testService.GetTests()));
        }
    }
}