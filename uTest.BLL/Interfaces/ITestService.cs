using System.Collections.Generic;
using uTest.BLL.DTO;

namespace uTest.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with BLL from Presentation layer (WEB)
    /// </summary>
    public interface ITestService
    {
        #region Create

        void CreateTest(TestDTO test);
        #endregion

        #region Update

        void UpdateTest(TestDTO test);

        #endregion

        #region Delete

        void DeleteTest(long id);

        #endregion

        #region Get

        TestDTO GetTest(long id);

        IEnumerable<TestDTO> GetTests();

        /// <summary>
        /// Filters tests by comparing 'searchString' arg with test name & description
        /// </summary>
        /// <param name="searchString">Search request</param>
        /// <returns>Filtred tests collection</returns>
        IEnumerable<TestDTO> GetTests(string searchString);

        #endregion

        void Dispose();
    }
}
