namespace uTest.BLL.DTO
{
    public class SolvedTestDTO
    {
        public int Id { get; set; }

        public int Result { get; set; }

        public string UserId { get; set; }

        public virtual TestDTO Test { get; set; }
    }
}
