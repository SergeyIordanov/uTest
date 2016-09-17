namespace uTest.BLL.DTO
{
    public class StatisticDTO
    {
        public string UserId { get; set; }

        public int SolvedTests { get; set; }

        public int CompletedTasks { get; set; }

        public int FailedTasks { get; set; }

        public int AvarageMark { get; set; }
    }
}
