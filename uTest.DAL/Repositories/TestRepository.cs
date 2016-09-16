using System;
using System.Collections.Generic;
using System.Linq;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class TestRepository : IRepository<Test>
    {

        private readonly TestContext _db;

        public TestRepository(TestContext context)
        {
            _db = context;
        }

        public IEnumerable<Test> GetAll()
        {
            return _db.Tests.ToList();
        }

        public Test Get(long id)
        {
            return _db.Tests.Find(id);
        }

        public void Create(Test test)
        {
            _db.Tests.Add(test);
        }

        public void Update(Test test)
        {
            Test original = _db.Tests.Find(test.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(test);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Test> Find(Func<Test, bool> predicate)
        {
            return _db.Tests.Where(predicate).ToList();
        }

        public void Delete(long id)
        {
            Test car = _db.Tests.Find(id);
            if (car != null)
                _db.Tests.Remove(car);
        }
    }
}
