using System.Collections.Generic;
using System.Linq;
using uTest.BLL.DTO;
using uTest.BLL.Infrastructure;
using uTest.BLL.Interfaces;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

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
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User id cannot be empty", "");
            var solvedTest = new SolvedTest { Result = result, Test = test, UserId = userId };
            Database.SolvedTests.Create(solvedTest);
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
            Database.Questions.Create(question);
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
            Database.Answers.Create(answer);
            question.Answers.Add(answer);
            Database.Questions.Update(question);
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

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
