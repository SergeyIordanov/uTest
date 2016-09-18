using System.Collections.Generic;
using System.Data.Entity;
using uTest.Entities.General;

namespace uTest.DAL.EF
{
    public class TestDbInitializer : DropCreateDatabaseAlways<TestContext>
    {
        protected override void Seed(TestContext db)
        {
            db.Tests.Add(new Test
            {
                Id = 1,
                IsPrivate = false,
                Name = "Test #1",
                Description = "Some text. Some text.\nSome text. Some text",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = 1,
                        IsMultipleAnswers = false,
                        Text = "Choose answer #3",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 1, Text = "Answer #1", IsCorrect = false},
                            new Answer {Id = 2, Text = "Answer #2", IsCorrect = false},
                            new Answer {Id = 3, Text = "Answer #3", IsCorrect = true}
                        }
                    },
                    new Question
                    {
                        Id = 2,
                        IsMultipleAnswers = true,
                        Text = "Choose answers #1 & #3",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 4, Text = "Answer #1", IsCorrect = true},
                            new Answer {Id = 5, Text = "Answer #2", IsCorrect = false},
                            new Answer {Id = 6, Text = "Answer #3", IsCorrect = true}
                        }
                    },
                    new Question
                    {
                        Id = 3,
                        IsMultipleAnswers = true,
                        Text = "Can you solve it?",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 7, Text = "Correct answer", IsCorrect = true}
                        }
                    },
                    new Question
                    {
                        Id = 2,
                        IsMultipleAnswers = false,
                        Text = "Choose answer #3",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 4, Text = "Answer #1", IsCorrect = false},
                            new Answer {Id = 5, Text = "Answer #2", IsCorrect = false},
                            new Answer {Id = 6, Text = "Answer #3", IsCorrect = true}
                        }
                    },
                }
            });

            db.Tests.Add(new Test
            {
                Id = 2,
                IsPrivate = false,
                Name = "Test #2",
                Description = null,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = 4,
                        IsMultipleAnswers = true,
                        Text = "Choose all",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 8, Text = "Answer #1", IsCorrect = true},
                            new Answer {Id = 9, Text = "Answer #2", IsCorrect = true},
                            new Answer {Id = 10, Text = "Answer #3", IsCorrect = true},
                            new Answer {Id = 11, Text = "Answer #4", IsCorrect = true},
                            new Answer {Id = 12, Text = "Answer #5", IsCorrect = true},
                            new Answer {Id = 13, Text = "Answer #6", IsCorrect = true}
                        }
                    },
                    new Question
                    {
                        Id = 5,
                        IsMultipleAnswers = false,
                        Text = "Choose #1",
                        Answers = new List<Answer>
                        {
                            new Answer {Id = 14, Text = "Answer #1", IsCorrect = true},
                            new Answer {Id = 15, Text = "Answer #2", IsCorrect = false},
                            new Answer {Id = 16, Text = "Answer #3", IsCorrect = false}
                        }
                    }
                }
            });

            db.SaveChanges();
        }
    }
}
