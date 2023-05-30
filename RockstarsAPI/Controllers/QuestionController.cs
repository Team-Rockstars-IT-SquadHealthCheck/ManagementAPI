using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data.SqlClient;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public QuestionController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [HttpPost]
        [Route("/Question")]
        public async Task<IActionResult> CreateQuestion(string question, string description, int surveyid, string desc_good, string desc_avg, string desc_bad)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO question (question, description, surveyid, desc_good, desc_avg, desc_bad) VALUES (@question, @description, @surveyid, @desc_good, @desc_avg, @desc_bad)", conn);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@surveyid", surveyid);
                cmd.Parameters.AddWithValue("@desc_good", desc_good);
                cmd.Parameters.AddWithValue("@desc_avg", desc_avg);
                cmd.Parameters.AddWithValue("@desc_bad", desc_bad);

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
