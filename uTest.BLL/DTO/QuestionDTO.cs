using System.Collections.Generic;

namespace uTest.BLL.DTO
{
    public class QuestionDTO
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsMultipleAnswers { get; set; }

        public virtual ICollection<AnswerDTO> Answers { get; set; }
    }
}
