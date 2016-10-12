using System;
using System.Collections.Generic;
using System.Linq;
using uTest.DAL.EF;
using uTest.DAL.Interfaces;
using uTest.Entities.General;

namespace uTest.DAL.Repositories
{
    public class TaskRepository : IRepository<Task>
    {
        private readonly TestContext _db;

        public TaskRepository(TestContext context)
        {
            _db = context;
        }

        public IEnumerable<Task> GetAll()
        {
            return _db.Tasks.ToList();
        }

        public Task Get(long id)
        {
            return _db.Tasks.Find(id);
        }

        public void Create(Task task)
        {
            _db.Tasks.Add(task);
        }

        public void Update(Task task)
        {
            Task original = _db.Tasks.Find(task.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(task);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Task> Find(Func<Task, bool> predicate)
        {
            return _db.Tasks.ToList().Where(predicate).ToList();
        }

        public void Delete(long id)
        {
            Task task = _db.Tasks.Find(id);
            if (task != null)
                _db.Tasks.Remove(task);
        }
    }
}

