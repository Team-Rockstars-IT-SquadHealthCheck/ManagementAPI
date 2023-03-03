namespace RockstarsAPI.models
{
    public class user
    {
        public int id { get;  set; }
        public string username { get;  set; }
        public string password { get;  set; }
        public string email { get;  set; }
        public int? roleid { get;  set; }
        public int? squadid { get;  set; }
        public int? answerid { get;  set; }
    }
}
