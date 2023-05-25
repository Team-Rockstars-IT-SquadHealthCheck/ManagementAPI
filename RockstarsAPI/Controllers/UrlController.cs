using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Tables.Item.ConvertToRange;
using RockstarsAPI.models;
using System.Data;
using System.Data.SqlClient;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {

        public readonly IConfiguration _Configuration;
        public UrlController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [HttpGet]
        [Route("/Urls")]
        public List<Url> Urls()
        {
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Url]", conn);
            DataTable datatable = new DataTable();
            adapter.Fill(datatable);
            List<Url> urlList = new List<Url>();
            if (datatable.Rows.Count > 0)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    Url url = new Url();
                    url.id = Convert.ToInt32(datatable.Rows[i]["id"]);
                    url.url = Convert.ToString(datatable.Rows[i]["url"]);
                    url.userid = Convert.ToInt32(datatable.Rows[i]["userid"]);
                    url.status = Convert.ToBoolean(datatable.Rows[i]["status"]);
                    urlList.Add(url);
                }
            }
            return urlList;
        }
        [HttpGet]
        [Route("/Url/{id}")]
        public Url Url(int id)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [url] WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Url url = new Url
                    {
                        id = (int)reader["id"],
                        url = (string)reader["url"],
                        userid = (int)reader["userid"],
                        status = Convert.ToBoolean(reader["status"])

                    };
                    return url;
                }
            }
            return null;
        }
        [HttpPost]
        [Route("/Url")]
        public IActionResult Save([FromBody] PostUrl url)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [url] (url, userid, status) VALUES (@url, @userid, 0)", conn);
                cmd.Parameters.AddWithValue("@url", url.url);
                cmd.Parameters.AddWithValue("@userid", url.userid);
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
