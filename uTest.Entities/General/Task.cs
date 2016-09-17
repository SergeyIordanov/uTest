using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uTest.Entities.General
{
    public class Task
    {
        [Key]
        [ForeignKey("SolvedTest")]
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
