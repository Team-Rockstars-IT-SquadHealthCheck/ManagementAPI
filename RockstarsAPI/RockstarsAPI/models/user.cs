﻿using System.ComponentModel.DataAnnotations;

namespace RockstarsAPI.models
{
    public class User
    {
        [Key]
        public int id { get;  set; }
        public string username { get;  set; }
        public string password { get;  set; }
        public string email { get;  set; }
        public int? roleid { get;  set; }
        public int? squadid { get;  set; }
        public int? answerid { get;  set; }
    }
}
