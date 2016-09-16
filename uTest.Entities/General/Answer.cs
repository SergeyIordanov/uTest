using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class Answer
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        public virtual Question Question { get; set; }
    }
}
