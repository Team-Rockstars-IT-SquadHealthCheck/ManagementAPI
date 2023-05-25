namespace RockstarsAPI.models
{
    public class AnswerCompany
    {

        public int Id { get; set; }
        public string question { get; set; }
        public int answer { get; set; }
        public string answerText { get; set; }
        public string comment { get; set; }
        public int userid { get; set; }
        public int squadid { get; set; }

    }
}
