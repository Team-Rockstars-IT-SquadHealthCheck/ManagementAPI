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
        public async Task<IActionResult> CreateQuestion([FromBody] Question question)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO question (question, description, surveyid, desc_good, desc_avg, desc_bad) VALUES (@question, @description, @surveyid, @desc_good, @desc_avg, @desc_bad)", conn);
                cmd.Parameters.AddWithValue("@question", question.question);
                cmd.Parameters.AddWithValue("@description", question.description);
                cmd.Parameters.AddWithValue("@surveyid", question.surveyid);
                cmd.Parameters.AddWithValue("@desc_good", question.desc_good);
                cmd.Parameters.AddWithValue("@desc_avg", question.desc_avg);
                cmd.Parameters.AddWithValue("@desc_bad", question.desc_bad);

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
