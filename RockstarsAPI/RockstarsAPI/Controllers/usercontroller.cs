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
    public class UserController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public UserController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet]
        [Route("/GetAllUsers")]
        public List<User> getuser()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM \"user\"", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<User> userList = new List<User>();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    User user = new User();
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

        [HttpGet]
        [Route("/{id}/details")]
        public User GetDetails(int? id)
        {
            User user = new User();
            user = GetUserInfo(id);

            return user;
        }

        [HttpGet]
        private User GetUserInfo(int? id)
        {
            User user = new User();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM \"user\" WHERE id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<User> userList = new List<User>();
            if (datatableuser.Rows.Count > 0)
            {
                user.id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                user.username = Convert.ToString(datatableuser.Rows[0]["username"]);
                user.password = Convert.ToString(datatableuser.Rows[0]["password"]);
                try
                {
                    user.roleid = Convert.ToInt32(datatableuser.Rows[0]["roleid"]);
                }
                catch (Exception e)
                {
                    Nullable<int> x = null;
                    user.roleid = x;
                }
                try
                {
                    user.squadid = Convert.ToInt32(datatableuser.Rows[0]["squadid"]);
                }
                catch (Exception e)
                {
                    Nullable<int> x = null;
                    user.squadid = x;
                }
                try
                {
                    user.answerid = Convert.ToInt32(datatableuser.Rows[0]["answerid"]);
                }
                catch (Exception e)
                {
                    Nullable<int> x = null;
                    user.answerid = x;
                }
            }

                return user;
        }
    }
}
