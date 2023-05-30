namespace RockstarsAPI.models
{
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public string description { get; set; }
        public int surveyid { get; set; }
        public string desc_good { get; set; }
        public string desc_avg { get; set; }
        public string desc_bad { get; set; }
    }
}
