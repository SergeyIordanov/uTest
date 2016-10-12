using System;
using System.Collections.Generic;
using System.Linq;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {

        private readonly TestContext _db;

        public QuestionRepository(TestContext context)
        {
            _db = context;
        }

        public IEnumerable<Question> GetAll()
        {
            return _db.Questions.ToList();
        }

        public Question Get(long id)
        {
            return _db.Questions.Find(id);
        }

        public void Create(Question question)
        {
            _db.Questions.Add(question);
        }

        public void Update(Question question)
        {
            Question original = _db.Questions.Find(question.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(question);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Question> Find(Func<Question, bool> predicate)
        {
            return _db.Questions.ToList().Where(predicate).ToList();
        }

        public void Delete(long id)
        {
            Question question = _db.Questions.Find(id);
            if (question != null)
                _db.Questions.Remove(question);
        }
    }
}
