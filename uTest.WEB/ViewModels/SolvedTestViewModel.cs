namespace uTest.WEB.ViewModels
{
    public class SolvedTestViewModel
    {
        public int Id { get; set; }

        public int Result { get; set; }

        public string UserId { get; set; }

        public virtual TestViewModel Test { get; set; }
    }
}