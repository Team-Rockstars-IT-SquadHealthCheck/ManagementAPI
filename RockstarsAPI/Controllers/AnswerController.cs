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
                    answer.answerText = GetAnswerText(answer.Id, answer.answer);

					
					answers.Add(answer);
				}
			}
			return answers;
		}
        [HttpGet]
        [Route("/Answer/Squad/{squadid}")]
        public List<Answer> GetAnswerFromSquad(int? squadid)
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT a.id, q.question, a.answer, a.comment, u.username " +
                "FROM answer a " +
                "JOIN [user] u on a.userid = u.id " +
                "JOIN [question] q ON a.questionid = q.id " +
                "WHERE u.squadid = @squadid ", conn);
            adapter.SelectCommand.Parameters.Add("@squadid", SqlDbType.Int).Value = squadid;
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

        private string GetAnswerText(int answerId, int answer)
        {
            string query = "";
            string answerText = "";

            switch (answer)
            {
                case 0:
                    query = "SELECT q.desc_good " +
                        "FROM answer a " +
                        "JOIN question q ON a.questionid = q.id " +
                        "WHERE a.id = @answerId ";
                    break;
                case 1:
                    query = "SELECT q.desc_avg " +
                        "FROM answer a " +
                        "JOIN question q ON a.questionid = q.id " +
                        "WHERE a.id = @answerId ";
                    break;
                case 2:
                    query = "SELECT q.desc_bad " +
                        "FROM answer a " +
                        "JOIN question q ON a.questionid = q.id " +
                        "WHERE a.id = @answerId ";
                    break;
            }

            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.Add("@answerId", SqlDbType.Int).Value = answerId;
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {

                    answerText = Convert.ToString(datatableuser.Rows[i][0]);


                    return answerText;
                }
            }


            return answerText;
        }

    }
}
