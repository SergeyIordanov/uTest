using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uTest.WEB.ViewModels
{
    public class TestViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        public virtual ICollection<QuestionViewModel> Questions { get; set; }

        public List<SolvedTestViewModel> SolvedTests { get; set; }
    }
}