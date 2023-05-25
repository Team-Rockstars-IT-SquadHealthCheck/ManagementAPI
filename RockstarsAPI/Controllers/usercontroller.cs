
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Graph.Models;
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
        [Route("/Users")]
        public List<User> AllUsers()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [user].id, username, email, password, roleid, role.name AS role, squadid, squad.name AS squad FROM [user] LEFT JOIN role ON [user].roleid = role.id LEFT JOIN squad ON [user].squadid = squad.id", conn);
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
                    user.email = Convert.ToString(datatableuser.Rows[i]["email"]);
                    user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    user.rolename = Convert.ToString(datatableuser.Rows[i]["role"]);
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
                        user.squadname = Convert.ToString(datatableuser.Rows[i]["squad"]);
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
        [Route("/UserDetails/{id}")]
        public User Details(int? id)
        {
            User user = new User();
            user = UserInfo(id);

            return user;
        }

        [HttpGet]
        private User UserInfo(int? id)
        {
            User user = new User();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT [user].id, username, email, password, roleid, role.name AS role, squadid, squad.name AS squad FROM [user] LEFT JOIN role ON [user].roleid = role.id LEFT JOIN squad ON [user].squadid = squad.id WHERE [user].id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                user.id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                user.username = Convert.ToString(datatableuser.Rows[0]["username"]);
                user.password = Convert.ToString(datatableuser.Rows[0]["password"]);
                user.email = Convert.ToString(datatableuser.Rows[0]["email"]);
                user.roleid = Convert.ToInt32(datatableuser.Rows[0]["roleid"]);
                user.rolename = Convert.ToString(datatableuser.Rows[0]["roleid"]);
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
                    user.squadname = Convert.ToString(datatableuser.Rows[0]["squad"]);
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
        [Route("/UsersInSquad/{squadid}")]
        public List<User> UsersInSquad(int? squadid)
        {
            List<User> users = new List<User>();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT [user].id, username, email, password, roleid, role.name AS role, squadid, squad.name AS squad FROM [user] LEFT JOIN role ON [user].roleid = role.id LEFT JOIN squad ON [user].squadid = squad.id WHERE squadid = {squadid}", conn);
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
                    user.email = Convert.ToString(datatableuser.Rows[i]["email"]);
                    user.roleid = Convert.ToInt32(datatableuser.Rows[i]["roleid"]);
                    user.rolename = Convert.ToString(datatableuser.Rows[i]["role"]);
                    user.squadid = Convert.ToInt32(datatableuser.Rows[i]["squadid"]);
                    user.squadname = Convert.ToString(datatableuser.Rows[i]["squad"]);
                    users.Add(user);
                }
            }
            return users;
        }

        [HttpGet]
        [Route("/UsersInCompany/{companyid}")]
        public List<User> AllUsersInCompany(int? companyid)
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
                    user.email = Convert.ToString(datatableuser.Rows[i]["email"]);
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
        public IActionResult CreateNewUser([FromBody] PostUser user)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO \"user\" (username, password, email, roleid, squadid) VALUES (@username, @password, @email, @roleid, @squadid)", conn);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@roleid", user.roleid);
                cmd.Parameters.AddWithValue("@squadid", user.squadid);
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

        
        //[HttpPut]
        //[Route("/User/{id}/Url")]
        //public IActionResult UrlUser(int id, [FromBody] string url)
        //{
        //    using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand("UPDATE \"user\" SET url = @url WHERE id = @id", conn);
        //        cmd.Parameters.AddWithValue("@url", url);
        //        cmd.Parameters.AddWithValue("@id", id);
        //        int rowsAffected = cmd.ExecuteNonQuery();
        //        if (rowsAffected == 1)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return StatusCode(500);
        //        }
        //    }
        //}
        [HttpDelete]
        [Route("/User/{id}")]
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE [user] WHERE [user].id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
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