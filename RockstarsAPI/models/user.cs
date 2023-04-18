using System.ComponentModel.DataAnnotations;

namespace RockstarsAPI.models
{
    public class User
    {
        public int id { get;  set; }
        public string username { get;  set; }
        public string password { get;  set; }
        public string email { get;  set; }
        public int roleid { get;  set; }
        public string rolename { get; set; }
        public int? squadid { get; set; }
        public string squadname { get; set; }
        public string? url { get; set; }
    }
}
