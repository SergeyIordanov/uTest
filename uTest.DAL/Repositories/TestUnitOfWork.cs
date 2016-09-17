using System;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private readonly TestContext _db;
        private TestRepository _testRepository;
        private QuestionRepository _questionRepository;
        private AnswerRepository _answerRepository;
        private SolvedTestRepository _solvedTestRepository;
        private TaskRepository _taskRepository;


        public TestUnitOfWork(string connectionString)
        {
            _db = new TestContext(connectionString);
        }
        public IRepository<Test> Tests => _testRepository ?? (_testRepository = new TestRepository(_db));

        public IRepository<Question> Questions => _questionRepository ?? (_questionRepository = new QuestionRepository(_db));

        public IRepository<Answer> Answers => _answerRepository ?? (_answerRepository = new AnswerRepository(_db));

        public IRepository<SolvedTest> SolvedTests => _solvedTestRepository ?? (_solvedTestRepository = new SolvedTestRepository(_db));

        public IRepository<Task> Tasks => _taskRepository ?? (_taskRepository = new TaskRepository(_db));

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Frees db resources
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //All unmanaged resources was disposed so GC needn't to call finalize method
            GC.SuppressFinalize(this);
        }
    }
}
