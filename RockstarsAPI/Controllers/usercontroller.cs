using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data;
using System.Data.SqlClient;

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
        [Route ("/GetAllUsers")]
        public List<User> GetAllUsers()
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
                    user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    try
                    {
                        user.squadid = Convert.ToInt32(datatableuser.Rows[i]["squadid"]);
                    }
                    catch (Exception e)
                    {
                        Nullable<int> x = null;
                        user.squadid = x;
                    }
                    userList.Add(user);
                }
            }
            return userList;
        }

        [HttpGet]
        [Route("/{id}/UserDetails")]
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
            if (datatableuser.Rows.Count > 0)
            {
                user.id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                user.username = Convert.ToString(datatableuser.Rows[0]["username"]);
                user.password = Convert.ToString(datatableuser.Rows[0]["password"]);
                user.roleid = Convert.ToInt32(datatableuser.Rows[0]["roleid"]);
                try
                {
                    user.squadid = Convert.ToInt32(datatableuser.Rows[0]["squadid"]);
                }
                catch (Exception e)
                {
                    Nullable<int> x = null;
                    user.squadid = x;
                }
            }
                return user;
        }

        [HttpGet]
        [Route("/{squadid}/UsersInSquad")]
        public List<User> GetUsersInSquad(int? squadid)
        {
            List<User> users = new List<User>();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM \"user\" WHERE squadid = {squadid}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    User user = new User();
                    user.id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    user.username = Convert.ToString(datatableuser.Rows[i]["username"]);
                    user.password = Convert.ToString(datatableuser.Rows[i]["password"]);
                    user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    user.squadid = Convert.ToInt32(datatableuser.Rows[i]["squadid"]);
                    users.Add(user);

                }
            }
            return users;   
        }

        [HttpGet]
        [Route("/{companyid}/GetAllUsersInCompany")]
        public List<User> GetAllUsersInCompany(int? companyid)
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT u.id , u.username, u.squadid, u.\"password\", u.roleid, u.email " +
                $"FROM \"user\" u " +
                $"JOIN squad s ON u.squadid = s.id " +
                $"WHERE s.companyid = {companyid} ", conn);
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
                    user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    try
                    {
                        user.squadid = Convert.ToInt32(datatableuser.Rows[i]["squadid"]);
                    }
                    catch (Exception e)
                    {
                        Nullable<int> x = null;
                        user.squadid = x;
                    }
                    userList.Add(user);
                }
            }
            return userList;
        }


        [HttpPost]
        public IActionResult CreateNewUser([FromBody] User user)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO \"user\" (username, password, email, roleid, squadid) VALUES (@username, @password, @email, @roleid, @squadid)", conn);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@roleid", user.roleid);
                cmd.Parameters.AddWithValue("@squadid", (object)user.squadid ?? DBNull.Value);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }
        
    }
}
