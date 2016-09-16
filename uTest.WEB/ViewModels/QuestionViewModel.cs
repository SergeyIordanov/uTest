using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uTest.WEB.ViewModels
{
    public class QuestionViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsMultipleAnswers { get; set; }

        public virtual ICollection<AnswerViewModel> Answers { get; set; }
    }
}