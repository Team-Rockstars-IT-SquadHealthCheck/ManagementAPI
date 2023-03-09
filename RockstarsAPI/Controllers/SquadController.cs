using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data;
using System.Data.SqlClient;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public SquadController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [HttpGet]
        [Route("/GetAllSquads")]
        public List<Squad> GetAllSquads()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM squad", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<Squad> squadList = new List<Squad>();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    Squad squad = new Squad();
                    squad.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    squad.SurveyId = Convert.ToInt32(datatableuser.Rows[i]["surveyid"]);
                    squad.CompanyId = Convert.ToInt32(datatableuser.Rows[i]["companyid"]);
                    squadList.Add(squad);

                }
            }
            return squadList;
        }

        [HttpGet]
        [Route("/{id}/SquadDetails")]
        public Squad GetDetails(int? id)
        {
            Squad squad = new Squad();
            squad = GetSquadInfo(id);

            return squad;
        }

        [HttpGet]
        private Squad GetSquadInfo(int? id)
        {
            Squad squad = new Squad();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM squad WHERE id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                squad.Id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                squad.SurveyId = Convert.ToInt32(datatableuser.Rows[0]["surveyid"]);
                squad.CompanyId = Convert.ToInt32(datatableuser.Rows[0]["companyid"]);

            }
            return squad;
        }
        [HttpPost]
        public IActionResult CreateNewSquad([FromBody] Squad squad)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO squad (surveyid, companyid) VALUES (@surveyid, @companyid)", conn);
                cmd.Parameters.AddWithValue("@surveyid", squad.SurveyId);
                cmd.Parameters.AddWithValue("@companyid", squad.CompanyId);

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
