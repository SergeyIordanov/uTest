using System;
using System.Collections.Generic;
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
            var tests = mapper.Map<IEnumerable<TestViewModel>>(_testService.GetPublicTests());
            foreach (var test in tests)
            {
                test.SolvedTests =
                    mapper.Map<IEnumerable<SolvedTestViewModel>>(_testService.GetSolvedTests(test.Id)).ToList();
            }
            return View(tests);
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            TestViewModel testView;
            try
            {
                var test = _testService.GetTest(id);           
                var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
                testView = mapper.Map<TestViewModel>(test);
                testView.SolvedTests = mapper.Map<IEnumerable<SolvedTestViewModel>>(_testService.GetSolvedTests(testView.Id)).ToList();
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
            return View(testView);
        }

        [HttpGet]
        public ActionResult SolveTest(long id)
        {            
            TestDTO test;
            var rand = new Random();
            try
            {
                test = _testService.GetTest(id);
                var questionsToShow = new QuestionDTO[test.QuestionsToSolve];

                int k;
                for (int i = 0; i < questionsToShow.Length; i++)
                {
                    while (true)
                    {
                        k = rand.Next(test.Questions.Count);
                        if (!questionsToShow.Any(x => x != null && x.Id.Equals(test.Questions.ElementAt(k).Id)))
                        {
                            questionsToShow[i] = test.Questions.ElementAt(k);
                            break;
                        }
                    }
                }
                test.Questions = questionsToShow.ToList();
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
            if(answers != null)
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

            int questionPrice = 100 / test.QuestionsToSolve;
            int failedQuestions = test.QuestionsToSolve - rightQuestions;
            int res = 100 - failedQuestions * questionPrice;

            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            var solvedTestView = new SolvedTestViewModel {Test = mapper.Map<TestViewModel>(test), UserId = User.Identity.GetUserId(), Result = res};
            try
            {
                _testService.SolveTest(test.Id, User.Identity.GetUserId(), res);
            }
            catch (ValidationException)
            {               
                _testService.DeleteSolvedTests(testId, User.Identity.GetUserId());
                _testService.SolveTest(test.Id, User.Identity.GetUserId(), res);
            }
            ViewBag.Solved = rightQuestions;
            return View("TestResult", solvedTestView);
        }
    }
}