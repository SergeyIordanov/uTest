using System;
using System.Collections.Generic;
using System.Linq;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class SolvedTestRepository : IRepository<SolvedTest>
    {

        private readonly TestContext _db;

        public SolvedTestRepository(TestContext context)
        {
            _db = context;
        }

        public IEnumerable<SolvedTest> GetAll()
        {
            return _db.SolvedTests.ToList();
        }

        public SolvedTest Get(long id)
        {
            return _db.SolvedTests.Find(id);
        }

        public void Create(SolvedTest solvedTest)
        {
            _db.SolvedTests.Add(solvedTest);
        }

        public void Update(SolvedTest solvedTest)
        {
            SolvedTest original = _db.SolvedTests.Find(solvedTest.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(solvedTest);
                _db.SaveChanges();
            }
        }

        public IEnumerable<SolvedTest> Find(Func<SolvedTest, bool> predicate)
        {
            return _db.SolvedTests.Where(predicate).ToList();
        }

        public void Delete(long id)
        {
            SolvedTest solvedTest = _db.SolvedTests.Find(id);
            if (solvedTest != null)
                _db.SolvedTests.Remove(solvedTest);
        }
    }
}
