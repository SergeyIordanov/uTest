using System;
using System.Collections.Generic;
using System.Linq;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class AnswerRepository : IRepository<Answer>
    {

        private readonly TestContext _db;

        public AnswerRepository(TestContext context)
        {
            _db = context;
        }

        public IEnumerable<Answer> GetAll()
        {
            return _db.Answers.ToList();
        }

        public Answer Get(long id)
        {
            return _db.Answers.Find(id);
        }

        public void Create(Answer answer)
        {
            _db.Answers.Add(answer);
        }

        public void Update(Answer answer)
        {
            Answer original = _db.Answers.Find(answer.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(answer);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Answer> Find(Func<Answer, bool> predicate)
        {
            return _db.Answers.ToList().Where(predicate).ToList();
        }

        public void Delete(long id)
        {
            Answer answer = _db.Answers.Find(id);
            if (answer != null)
                _db.Answers.Remove(answer);
        }
    }
}
