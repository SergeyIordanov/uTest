using System;
using System.Collections.Generic;
using System.Linq;
using uTest.BLL.DTO;
using uTest.BLL.Infrastructure;
using uTest.BLL.Interfaces;
using uTest.DAL.Interfaces;
using uTest.Entities.General;
using Word = Microsoft.Office.Interop.Word;

namespace uTest.BLL.Services
{
    public class TestService : ITestService
    {

        private IUnitOfWork Database { get; }

        public TestService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void SolveTest(long testId, string userId, int result)
        {
            var test = Database.Tests.Get(testId);
            if (test == null)
                throw new ValidationException("Cannot get test for solving", "");
            if (test.SolvedTests.Any(x => x.UserId.Equals(userId)))
                throw new ValidationException("Test was already solved by this user", "");
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User id cannot be empty", "");
            var solvedTest = new SolvedTest { Result = result, Test = test, UserId = userId };
            Database.SolvedTests.Create(solvedTest);
            Database.Save();
        }

        public void SolveTask(TaskDTO taskDTO, int result)
        {
            Validator.ValidateTaskModel(taskDTO);
            if (taskDTO.IsSolved)
                throw new ValidationException("Task was already solved", "");
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var task = mapper.Map<Task>(taskDTO);
            task.IsSolved = true;
            var solvedTest = new SolvedTest
            {
                Result = result,
                UserId = taskDTO.UserId,
                Task = Database.Tasks.Get(task.Id),
                Test = Database.Tests.Get(task.Test.Id)
            };
            Database.SolvedTests.Create(solvedTest);         
            task.SolvedTest = solvedTest;
            Database.Tasks.Update(task);
            Database.Save();
        }

        #region Create

        public void CreateTest(TestDTO testDTO)
        {
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateTestModel(testDTO);
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var test = mapper.Map<Test>(testDTO);
            // Creating and saving
            Database.Tests.Create(test);
            Database.Save();
        }

        public void CreateTestFromDoc(string path)
        {
            Word.Application app = null;
            Word.Document doc = null;
            object nullobj = System.Reflection.Missing.Value;
            try
            {
                var testDTO = new TestDTO {Questions = new List<QuestionDTO>() };

                app = new Word.Application();             
                object file = path;
                doc = app.Documents.Open(
                    ref file, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj,
                    ref nullobj, ref nullobj, ref nullobj);

                Word.Table testTable = doc.Tables[1];

                if (testTable.Rows[1].Cells.Count == 1)
                {
                    testDTO.Name = testTable.Cell(1, 1).Range.Text.Replace("\r\a", string.Empty);
                }
                else
                    throw new ValidationException("Wrong doc structure", "");
                if (testTable.Rows[2].Cells.Count == 1)
                {
                    testDTO.Description = testTable.Cell(2, 1).Range.Text.Replace("\r\a", string.Empty);
                }
                else
                    throw new ValidationException("Wrong doc structure", "");

                var questionsPos = new Dictionary<int, string>();
                for (int i = 3; i < testTable.Rows.Count; i++)
                {
                    if (testTable.Rows[i].Cells.Count == 1 && testTable.Rows[i + 1].Cells.Count == 1)
                        questionsPos.Add(i + 1, testTable.Cell(i + 1, 1).Range.Text.Replace("\r\a", string.Empty));
                }
                foreach (KeyValuePair<int, string> pair in questionsPos)
                {
                    int correctsCount = 0;
                    var question = new QuestionDTO {Text = pair.Value, Answers = new List<AnswerDTO>()};
                    for (int i = pair.Key+1; i <= testTable.Rows.Count; i++)
                    {
                        if (testTable.Rows[i].Cells.Count == 1)
                            break;
                        string text = testTable.Cell(i, 2).Range.Text.Replace("\r\a", string.Empty);
                        string correct = testTable.Cell(i, 1).Range.Text.Replace("\r\a", string.Empty);
                        var answer = new AnswerDTO
                        {
                            Text = text,
                            IsCorrect = correct.Equals("+")
                        };
                        if (answer.IsCorrect)
                            correctsCount++;
                        question.Answers.Add(answer);
                    }
                    if (correctsCount > 1)
                        question.IsMultipleAnswers = true;                    
                    testDTO.Questions.Add(question);
                }
                testDTO.QuestionsToSolve = testDTO.Questions.Count;
                // Using ValidationException for transfer validation data to presentation layer
                Validator.ValidateTestModel(testDTO);
                // Mapping DTO object into DB entity
                var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
                var test = mapper.Map<Test>(testDTO);
                // Creating and saving
                Database.Tests.Create(test);
                Database.Save();
            }
            catch (ValidationException ex)
            {
                throw new ValidationException("Document parsing failed. Details: " + ex.Message + ". " + ex.Property, "");
            }
            catch (Exception)
            {
                throw new ValidationException("Document parsing failed. Invalid structure", "");
            }
            finally
            {
                doc?.Close(ref nullobj, ref nullobj, ref nullobj);
                app?.Quit(ref nullobj, ref nullobj, ref nullobj);
            }
        }

        public void CreateQuestion(QuestionDTO questionDTO, long testId)
        {
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateQuestionModel(questionDTO);
            var test = Database.Tests.Get(testId);
            if(test == null)
                throw new ValidationException("Cannot get test for adding a question", "");
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var question = mapper.Map<Question>(questionDTO);
            // Creating and saving
            test.Questions.Add(question);
            Database.Tests.Update(test);
            Database.Save();
        }

        public void CreateAnswer(AnswerDTO answerDTO, long questionId)
        {
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateAnswerModel(answerDTO);
            var question = Database.Questions.Get(questionId);
            if (question == null)
                throw new ValidationException("Cannot get question for adding an answer", "");
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var answer = mapper.Map<Answer>(answerDTO);
            // Creating and saving
            question.Answers.Add(answer);
            Database.Questions.Update(question);
            Database.Save();
        }

        public void CreateTask(TaskDTO taskDTO, long testId)
        {
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateTaskModel(taskDTO);
            var test = Database.Tests.Get(testId);
            if (test == null)
                throw new ValidationException("Cannot get test for adding a task", "");
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var task = mapper.Map<Task>(taskDTO);
            // Creating and saving
            test.Tasks.Add(task);
            Database.Tests.Update(test);
            Database.Save();
        }

        #endregion

        #region Update

        public void UpdateTest(TestDTO testDto)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Tests.Get(testDto.Id) == null)
                throw new ValidationException("Test wasn't found", "");
            Validator.ValidateTestModel(testDto);
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var test = mapper.Map<Test>(testDto);
            // Updating & saving
            Database.Tests.Update(test);
            Database.Save();
        }

        public void UpdateQuestion(QuestionDTO questionDto)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Questions.Get(questionDto.Id) == null)
                throw new ValidationException("Question wasn't found", "");
            Validator.ValidateQuestionModel(questionDto);
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var question = mapper.Map<Question>(questionDto);
            // Updating & saving
            Database.Questions.Update(question);
            Database.Save();
        }

        public void UpdateAnswer(AnswerDTO answerDto)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Answers.Get(answerDto.Id) == null)
                throw new ValidationException("Answer wasn't found", "");
            Validator.ValidateAnswerModel(answerDto);
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var answer = mapper.Map<Answer>(answerDto);
            // Updating & saving
            Database.Answers.Update(answer);
            Database.Save();
        }

        public void UpdateTask(TaskDTO taskDTO)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Tasks.Get(taskDTO.Id) == null)
                throw new ValidationException("Task wasn't found", "");
            Validator.ValidateTaskModel(taskDTO);
            // Mapping DTO object into DB entity
            var mapper = MapperConfig.GetConfigFromDTO().CreateMapper();
            var task = mapper.Map<Task>(taskDTO);
            // Updating & saving
            Database.Tasks.Update(task);
            Database.Save();
        }

        #endregion

        #region Delete

        public void DeleteTest(long id)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (!Database.Tests.Find(x => x.Id == id).Any())
                throw new ValidationException("Test wasn't found", "");
            // Deleting & saving
            Database.Tests.Delete(id);
            Database.Save();
        }

        public void DeleteQuestion(long id)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (!Database.Questions.Find(x => x.Id == id).Any())
                throw new ValidationException("Question wasn't found", "");
            // Deleting & saving
            Database.Questions.Delete(id);
            Database.Save();
        }

        public void DeleteAnswer(long id)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (!Database.Answers.Find(x => x.Id == id).Any())
                throw new ValidationException("Answer wasn't found", "");
            // Deleting & saving
            Database.Answers.Delete(id);
            Database.Save();
        }

        public void DeleteTask(long id)
        {
            // Using ValidationException for transfer validation data to presentation layer
            if (!Database.Tasks.Find(x => x.Id == id).Any())
                throw new ValidationException("Task wasn't found", "");
            // Deleting & saving
            Database.Tasks.Delete(id);
            Database.Save();
        }

        public void DeleteSolvedTests(long testId, string userId)
        {
            var solvedTests = Database.SolvedTests.Find(x => x.Test.Id == testId && x.UserId.Equals(userId));
            foreach (var solvedTest in solvedTests)
            {
                Database.SolvedTests.Delete(solvedTest.Id);
            }
        }

        #endregion

        #region Get

        public TestDTO GetTest(long id)
        {
            // Using ValidationException for transfer validation data to presentation layer
            var test = Database.Tests.Get(id);
            if (test == null)
                throw new ValidationException("Test wasn't found", "");

            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();
            return mapper.Map<TestDTO>(test);
        }

        public IEnumerable<TestDTO> GetTests()
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();
            return mapper.Map<IEnumerable<TestDTO>>(Database.Tests.GetAll());
        }

        public IEnumerable<TestDTO> GetTests(string searchString)
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();
            // Returns all items if search string is empty or null
            if (string.IsNullOrEmpty(searchString))
                return mapper.Map<IEnumerable<TestDTO>>(Database.Tests.GetAll());

            return mapper.Map<IEnumerable<TestDTO>>(Database.Tests.Find(test => (test.Name + " " + test.Description).ToLower().Contains(searchString.ToLower())));
        }

        public IEnumerable<TestDTO> GetPublicTests()
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();

            return mapper.Map<IEnumerable<TestDTO>>(Database.Tests.Find(test => !test.IsPrivate));
        }

        public IEnumerable<SolvedTestDTO> GetSolvedTests(string userId)
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();
            // Returns all items if user id string is empty or null
            if (string.IsNullOrEmpty(userId))
                return mapper.Map<IEnumerable<SolvedTestDTO>>(Database.SolvedTests.GetAll());

            return mapper.Map<IEnumerable<SolvedTestDTO>>(Database.SolvedTests.Find(test => test.UserId.Equals(userId)));
        }

        public IEnumerable<SolvedTestDTO> GetSolvedTests(long testId)
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();

            return mapper.Map<IEnumerable<SolvedTestDTO>>(Database.SolvedTests.Find(test => test.Test.Id.Equals(testId)));
        }

        public IEnumerable<TaskDTO> GetTasks(string userId)
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();
            // Returns all items if user id string is empty or null
            if (string.IsNullOrEmpty(userId))
                return mapper.Map<IEnumerable<TaskDTO>>(Database.Tasks.GetAll());

            return mapper.Map<IEnumerable<TaskDTO>>(Database.Tasks.Find(task => task.UserId.Equals(userId)));
        }

        public IEnumerable<TaskDTO> GetTasks(long testId)
        {
            // Using of Automapper for projection the Car entity into the CarDTO
            var mapper = MapperConfig.GetConfigToDTO().CreateMapper();

            return mapper.Map<IEnumerable<TaskDTO>>(Database.Tasks.Find(task => task.Test.Id.Equals(testId)));
        }

        public StatisticDTO GetStatistic(string userId)
        {
            var result = new StatisticDTO {UserId = userId};
            var solvedTests = Database.SolvedTests.Find(x => x.UserId.Equals(userId));
            var tasks = Database.Tasks.Find(x => x.UserId.Equals(userId));
            var solvedTestsList = solvedTests as IList<SolvedTest> ?? solvedTests.ToList();
            if (solvedTests != null && solvedTestsList.Any())
            {
                result.SolvedTests = solvedTestsList.Count;
                result.AvarageMark = solvedTestsList.Sum(x => x.Result) / solvedTestsList.Count;
            }
            var tasksList = tasks as IList<Task> ?? tasks.ToList();
            if (tasks != null && tasksList.Any())
            {
                result.CompletedTasks = tasksList.Count(x => x.IsSolved && x.SolvedTest.Result >= x.MinResult);
                result.FailedTasks = tasksList.Count(x => x.IsSolved && x.SolvedTest.Result < x.MinResult);
            }
            return result;
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
