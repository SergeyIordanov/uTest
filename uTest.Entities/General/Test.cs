using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class Test
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<SolvedTest> SolvedTests { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
