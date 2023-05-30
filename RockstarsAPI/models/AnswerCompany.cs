namespace RockstarsAPI.models
{
    public class AnswerCompany
    {

        public int squadid { get; set; }
        public List<Answer> answerList { get; set;}
        public AnswerCompany()
        {
            answerList = new List<Answer>();
        }
    }
}
