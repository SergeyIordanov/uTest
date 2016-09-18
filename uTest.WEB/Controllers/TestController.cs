using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using uTest.BLL.DTO;
using uTest.BLL.Infrastructure;
using uTest.BLL.Interfaces;
using uTest.WEB.ViewModels;
using MapperConfig = uTest.WEB.Util.MapperConfig;

namespace uTest.WEB.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        readonly ITestService _testService;
        public TestController(ITestService serv)
        {
            _testService = serv;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Public()
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<IEnumerable<TestViewModel>>(_testService.GetPublicTests()));
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            TestDTO test;
            try
            {
                test = _testService.GetTest(id);
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<TestViewModel>(test));
        }

        [HttpGet]
        public ActionResult SolveTest(long id)
        {
            TestDTO test;
            try
            {
                test = _testService.GetTest(id);
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            return View(mapper.Map<TestViewModel>(test));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolveTest(string[] answers, long testId)
        {
            TestDTO test;
            try
            {
                test = _testService.GetTest(testId);
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
            var allAnswers = new List<string>();
            allAnswers.AddRange(answers);
            allAnswers.AddRange(from a in Request.Form.AllKeys where a.Contains("radio") select Request.Form[a]);

            int rightQuestions = 0;
            foreach (var question in test.Questions)
            {
                bool ok = true;
                foreach (var answer in question.Answers)
                {
                    if (answer.IsCorrect)
                    {
                        if (!allAnswers.Contains(answer.Id.ToString()))
                            ok = false;
                    }                   
                }
                if (ok)
                    rightQuestions++;
            }

            int questionPrice = 100 / test.Questions.Count;
            int failedQuestions = test.Questions.Count - rightQuestions;
            int res = 100 - failedQuestions * questionPrice;

            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            var solvedTestView = new SolvedTestViewModel {Test = mapper.Map<TestViewModel>(test), UserId = User.Identity.GetUserId(), Result = res};
            _testService.SolveTest(test.Id, User.Identity.GetUserId(), res);

            return RedirectToAction("TestResult", solvedTestView);
        }
    }
}