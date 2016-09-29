using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
        public ActionResult Create(TestViewModel testView, string[] correct)
        {
            int i = 1;
            if(correct == null)
                correct = new string[0];
            var questions = new List<QuestionViewModel>();
            while (true)
            {
                var questionText = Request.Form["question_" + i];
                if (questionText != null)
                {
                    var answers = new List<AnswerViewModel>();
                    int j = 1;
                    int correctCount = 0;
                    while (true)
                    {
                        var answerText = Request.Form["answer_" + i + "_" + j];
                        if (answerText != null)
                        {
                            var answer = new AnswerViewModel {Text = answerText};                            
                            foreach (string cor in correct)
                            {
                                if ((i + "_" + j).Equals(cor))
                                {
                                    answer.IsCorrect = true;
                                    correctCount++;
                                }
                            }
                            answers.Add(answer);
                            j++;
                        }
                        else
                            break;
                    }
                    questions.Add(new QuestionViewModel {Text = questionText, Answers = answers, IsMultipleAnswers = correctCount > 1});
                    i++;
                }
                else 
                    break;
            }
            testView.Questions = questions;
            if (Request.Form["showAll"] != null)
                testView.QuestionsToSolve = testView.Questions.Count;
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
        public ActionResult CreateFromDoc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFromDoc(HttpPostedFileBase uploadFile)
        {
            try
            {
                if (uploadFile != null)
                {
                    //validate file format
                    string[] fileNameArr = uploadFile.FileName.Split('.');
                    string path;
                    if (fileNameArr[fileNameArr.Length - 1].Equals("doc"))
                        path = Server.MapPath("~/App_Data/Docs/Doc.doc");
                    else if(fileNameArr[fileNameArr.Length - 1].Equals("docx"))
                        path = Server.MapPath("~/App_Data/Docs/Doc.docx");
                    else
                        throw new ValidationException("Wrong file format", "");
                    uploadFile.SaveAs(path);
                    _testService.CreateTestFromDoc(path);
                }
                else
                {
                    throw new ValidationException("File is required", "");
                }               
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("Cannot save your file. Details: " + ex.Message, "");
                return View();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
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

        public FileResult GetTemplate()
        {
            string filePath = Server.MapPath("~/App_Data/Templates/TemplateForTests.docx");
            string file_type = "application/docx";
            string file_name = "TemplateForTests.docx";
            return File(filePath, file_type, file_name);
        }
    }
}