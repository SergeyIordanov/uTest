using System.Data.Entity;
using uTest.Entities.General;

namespace uTest.DAL.EF
{
    public class TestContext : DbContext
    {
        public virtual DbSet<Test> Tests { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Answer> Answers { get; set; }

        /// <summary>
        /// Static constructor for setting DB initializer
        /// </summary>
        static TestContext()
        {
            Database.SetInitializer(new TestDbInitializer());
        }

        public TestContext() { }

        public TestContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>()
               .HasMany(a => a.Questions)
               .WithRequired(a => a.Test)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Question>()
               .HasMany(a => a.Answers)
               .WithRequired(a => a.Question)
               .WillCascadeOnDelete(true);
        }
    }
}
