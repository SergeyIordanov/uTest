using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class Task
    {
        public long Id { get; set; }

        public bool IsSolved { get; set; }

        public int MinResult { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual Test Test { get; set; }

        public virtual SolvedTest SolvedTest { get; set; }
    }
}
