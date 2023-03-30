using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data;
using System.Data.SqlClient;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public SurveyController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [HttpGet]
        [Route("/Surveys")]
        public List<Survey> AllSurvey()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM survey", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<Survey> surveyList = new List<Survey>();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    Survey survey = new Survey();
                    survey.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    survey.Name = Convert.ToString(datatableuser.Rows[i]["name"]);
                    survey.Description = Convert.ToString(datatableuser.Rows[i]["description"]);
                    surveyList.Add(survey);
                }
            }
            return surveyList;
        }

        [HttpGet]
        [Route("/SurveyDetails/{id}")]
        public Survey Details(int? id)
        {
            Survey survey = new Survey();
            survey = SurveyInfo(id);

            return survey;
        }

        [HttpGet]
        private Survey SurveyInfo(int? id)
        {
            Survey survey = new Survey();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM survey WHERE id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<User> userList = new List<User>();
            if (datatableuser.Rows.Count > 0)
            {
                survey.Id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                survey.Name = Convert.ToString(datatableuser.Rows[0]["name"]);
                survey.Description = Convert.ToString(datatableuser.Rows[0]["description"]);
                
            }
            return survey;
        }
        [HttpPost]
        public IActionResult NewSurvey([FromBody] Survey survey)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO survey (name, description) VALUES (@name, @description)", conn);
                cmd.Parameters.AddWithValue("@name", survey.Name);
                cmd.Parameters.AddWithValue("@description", survey.Description);
                
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
