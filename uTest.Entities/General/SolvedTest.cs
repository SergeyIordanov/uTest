using System.ComponentModel.DataAnnotations;
using uTest.Entities.Identity;

namespace uTest.Entities.General
{
    public class SolvedTest
    {
        public int Id { get; set; }

        [Required]
        public int Result { get; set; }

        [Required]
        public virtual ClientProfile ClientProfile { get; set; }

        [Required]
        public virtual Test Test { get; set; }
    }
}
