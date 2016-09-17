namespace uTest.BLL.DTO
{
    public class TaskDTO
    {
        public long Id { get; set; }

        public bool IsSolved { get; set; }

        public int MinResult { get; set; }

        public string UserId { get; set; }

        public virtual TestDTO Test { get; set; }

        public virtual SolvedTestDTO SolvedTest { get; set; }
    }
}
