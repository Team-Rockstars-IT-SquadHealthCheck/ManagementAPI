using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;
//using System.Text.Json;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usercontroller : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public usercontroller(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet]
        [Route("/GetAllUsers")]
        public List<user> getuser()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM \"user\"", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<user> userList = new List<user>();
            response response = new response();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    user user = new user();
                    user.id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    user.username = Convert.ToString(datatableuser.Rows[i]["username"]);
                    user.password = Convert.ToString(datatableuser.Rows[i]["password"]);
                    try
                    {
                        user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    }
                    catch (Exception e)
                    {
                        Nullable<int> x = null;
                        user.roleid = x;
                    }
                    try
                    {
                        user.squadid = Convert.ToInt32(datatableuser.Rows[i]["squadid"]);
                    }
                    catch (Exception e)
                    {
                        Nullable<int> x = null;
                        user.squadid = x;
                    }
                    try
                    {
                        user.answerid = Convert.ToInt32(datatableuser.Rows[i]["answerid"]);
                    }
                    catch (Exception e)
                    {
                        Nullable<int> x = null;
                        user.answerid = x;
                    }
                    userList.Add(user);
                }
            }
            return userList;
        }
    }
}
