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
       
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
