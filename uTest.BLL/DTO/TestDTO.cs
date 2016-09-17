using System.Collections.Generic;

namespace uTest.BLL.DTO
{
    public class TestDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<QuestionDTO> Questions { get; set; }
    }
}
