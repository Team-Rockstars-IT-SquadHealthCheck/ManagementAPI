using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using RockstarsAPI.models;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {

		public readonly IConfiguration _Configuration;
		public AnswerController(IConfiguration Configuration)
		{
			_Configuration = Configuration;
		}


		[HttpGet]
		[Route("/Answer/User/{userid}")]
		public List<Answer> GetAnswerFromUser(int? userid)
		{
			HttpContext.Response.Headers.Add("Content-Type", "application/json");
			HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
			SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
			SqlDataAdapter adapter = new SqlDataAdapter("SELECT a.id , q.question, a.answer, a.comment " +
				"FROM answer a " +
				"JOIN question q ON a.questionid = q.id " +
				"WHERE a.userid = @userid ", conn);
			adapter.SelectCommand.Parameters.Add("@userid", SqlDbType.Int).Value = userid;
			DataTable datatableuser = new DataTable();
			adapter.Fill(datatableuser);
			List<Answer> answers = new List<Answer>();	
			if (datatableuser.Rows.Count > 0)
			{
				for (int i = 0; i < datatableuser.Rows.Count; i++)
				{
					Answer answer = new Answer();
					answer.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
					answer.question = Convert.ToString(datatableuser.Rows[i]["question"]);
					answer.answer = Convert.ToInt32(datatableuser.Rows[i]["answer"]);
					answer.comment = Convert.ToString(datatableuser.Rows[i]["comment"]);

					
					answers.Add(answer);
				}
			}
			return answers;
		}
		
    }
}
