using System.ComponentModel.DataAnnotations;

namespace uTest.WEB.ViewModels
{
    public class AnswerViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
    }
}