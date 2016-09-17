using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class SolvedTest
    {
        public long Id { get; set; }

        [Required]
        public int Result { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual Test Test { get; set; }

        public virtual Task Task { get; set; }
    }
}
