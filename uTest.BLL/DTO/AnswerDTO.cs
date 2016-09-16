namespace uTest.BLL.DTO
{
    public class AnswerDTO
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public virtual QuestionDTO Question { get; set; }
    }
}
