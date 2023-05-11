using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data.SqlClient;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkSurveySquadController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public LinkSurveySquadController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }   

        [HttpPost]
        public IActionResult LinkSurveySquad([FromBody] LinkSurveySquad LinkSurveySquad)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO \"linksurveysquad\" (squadid, surveyid) VALUES (@squadid, @surveyid)", conn);
                cmd.Parameters.AddWithValue("@squadid", LinkSurveySquad.Squadid);
                cmd.Parameters.AddWithValue("@surveyid", LinkSurveySquad.Surveyid);
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


