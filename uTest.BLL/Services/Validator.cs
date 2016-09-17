using uTest.BLL.DTO;
using uTest.BLL.Infrastructure;

namespace uTest.BLL.Services
{
    public static class Validator
    {
        public static void ValidateTestModel(TestDTO testDto)
        {
            if (testDto == null)
                throw new ValidationException("Cannot create test from null", "");
            if (string.IsNullOrEmpty(testDto.Name))
                throw new ValidationException("This property cannot be empty", "Name");
        }

        public static void ValidateQuestionModel(QuestionDTO questionDto)
        {          
            if (questionDto == null)
                throw new ValidationException("Cannot create question from null", "");
            if (questionDto.Text == null)
                throw new ValidationException("This property cannot be null", "Text");           
        }

        public static void ValidateAnswerModel(AnswerDTO answerDto)
        {
            if (answerDto == null)
                throw new ValidationException("Cannot create answer from empty", "");
            if (answerDto.Text == null)
                throw new ValidationException("This property cannot be empty", "Text");
        }
    }
}
