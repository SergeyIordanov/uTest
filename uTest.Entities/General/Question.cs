using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uTest.Entities.General
{
    public class Question
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsMultipleAnswers { get; set; }

        [Required]
        public virtual Test Test { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
