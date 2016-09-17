using System.Collections.Generic;
using uTest.BLL.DTO;

namespace uTest.BLL.Interfaces
{
    /// <summary>
    /// Interface for working with BLL from Presentation layer (WEB)
    /// </summary>
    public interface ITestService
    {
        void SolveTest(long testId, string userId, int result);

        void SolveTask(TaskDTO task, int result);

        #region Create

        void CreateTest(TestDTO test);

        void CreateQuestion(QuestionDTO question, long testId);

        void CreateAnswer(AnswerDTO answer, long questionId);

        void CreateTask(TaskDTO task, long testId);

        #endregion

        #region Update

        void UpdateTest(TestDTO test);

        void UpdateQuestion(QuestionDTO question);

        void UpdateAnswer(AnswerDTO answer);

        void UpdateTask(TaskDTO task);

        #endregion

        #region Delete

        void DeleteTest(long id);

        void DeleteQuestion(long id);

        void DeleteAnswer(long id);

        void DeleteTask(long id);

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

        /// <summary>
        /// Gets SolvedTests for specified user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>SolvedTests collection</returns>
        IEnumerable<SolvedTestDTO> GetSolvedTests(string userId);

        /// <summary>
        /// Gets SolvedTests for specified test
        /// </summary>
        /// <param name="testId">test id</param>
        /// <returns>SolvedTests collection</returns>
        IEnumerable<SolvedTestDTO> GetSolvedTests(long testId);

        /// <summary>
        /// Gets tasks for specified user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>Tasks collection</returns>
        IEnumerable<TaskDTO> GetTasks(string userId);

        /// <summary>
        /// Gets tasks for specified test
        /// </summary>
        /// <param name="testId">test id</param>
        /// <returns>Tasks collection</returns>
        IEnumerable<TaskDTO> GetTasks(long testId);

        /// <summary>
        /// Gets user statistic
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>Class with statistic info</returns>
        StatisticDTO GetStatistic(string userId);

        #endregion

        void Dispose();
    }
}
