using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class SolvedTest
    {
        public int Id { get; set; }

        [Required]
        public int Result { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual Test Test { get; set; }
    }
}
