using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
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
        [Route("/Squads")]
        public List<Squad> AllSquads()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT [squad].id , companyid , company.name AS companyname ,[squad].name FROM squad LEFT JOIN company ON squad.companyid = company.id", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<Squad> squadList = new List<Squad>();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    Squad squad = new Squad();
                    squad.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    squad.name = Convert.ToString(datatableuser.Rows[i]["name"]);
					try
					{
						squad.CompanyId = Convert.ToInt32(datatableuser.Rows[i]["companyid"]);
					}
					catch (Exception e)
					{
						Nullable<int> x = null;
						squad.CompanyId = x;
                    }
                    try
                    {
						squad.CompanyName = Convert.ToString(datatableuser.Rows[i]["companyname"]);
					}
					catch (Exception e)
					{
                        squad.CompanyName = null;
					}

					squadList.Add(squad);

                }
            }
            return squadList;
        }

        [HttpGet]
        [Route("/SquadDetails/{id}")]
        public Squad Details(int? id)
        {
            Squad squad = new Squad();
            squad = SquadInfo(id);

            return squad;
        }

        [HttpGet]
        private Squad SquadInfo(int? id)
        {
            Squad squad = new Squad();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT [squad].id , companyid , company.name AS companyname ,[squad].name FROM squad LEFT JOIN company ON squad.companyid = company.id WHERE [squad].id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                squad.Id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                squad.name = Convert.ToString(datatableuser.Rows[0]["name"]);           
                try
				{
					squad.CompanyId = Convert.ToInt32(datatableuser.Rows[0]["companyid"]);
				}
				catch (Exception e)
				{
					Nullable<int> x = null;
					squad.CompanyId = x;
				}
				try
				{
					squad.CompanyName = Convert.ToString(datatableuser.Rows[0]["companyname"]);
				}
				catch (Exception e)
				{
					squad.CompanyName = null;
				}


			}
            return squad;
        }

        [HttpGet]
        [Route("/SquadsInCompany/{companyId}")]
        public List<Squad> SquadInCompany(int? companyId)
        {
            List<Squad> squads = new List<Squad>();

            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT [squad].id , companyid , company.name AS companyname ,[squad].name FROM squad LEFT JOIN company ON squad.companyid = company.id WHERE companyid = {companyId}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    Squad squad = new Squad();
                    squad.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    squad.name = Convert.ToString(datatableuser.Rows[i]["name"]);
                    					try
					{
						squad.CompanyId = Convert.ToInt32(datatableuser.Rows[i]["companyid"]);
					}
					catch (Exception e)
					{
						Nullable<int> x = null;
						squad.CompanyId = x;
                    }
                    try
                    {
						squad.CompanyName = Convert.ToString(datatableuser.Rows[i]["companyname"]);
					}
					catch (Exception e)
					{
                        squad.CompanyName = null;
					}


                    squads.Add(squad);
                }
            }

            return squads;
        }

        [HttpPost]
        [Route("/api/Squad")]
        public IActionResult NewSquad([FromBody] Squad squad)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO squad (name, companyid) VALUES (@name, @companyid)", conn);
                cmd.Parameters.AddWithValue("@name", squad.name);
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

        [HttpDelete]
        [Route("/api/Squad/{id}/Delete")] 
        public IActionResult DeleteSquad(int id)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("BEGIN TRAN; " +
                    "BEGIN TRY " +
                    "    UPDATE [user] " +
                    "    SET squadid = NULL " +
                    "    WHERE squadid = @SquadId; " +
                    "    DELETE FROM squad " +
                    "    WHERE id = @SquadId; " +
                    "    COMMIT TRAN; " +
                    "    PRINT 'Squad and associated users deleted successfully.'; " +
                    "END TRY " +
                    "BEGIN CATCH " +
                    "    ROLLBACK TRAN; " +
                    "    PRINT 'Error occurred while deleting the squad and associated users.'; " +
                    "END CATCH", conn);
                cmd.Parameters.AddWithValue("@SquadId", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected >= 1)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

		[HttpPost]
		[Route("/api/Squad/{squadId}/Company/{companyId}")]
		public IActionResult AddSquadToCompany(int squadId, int companyId)
		{
			using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand("UPDATE squad " +
					"SET companyid = @companyId " +
					"WHERE id = @squadId;", conn);
				cmd.Parameters.AddWithValue("@squadId", squadId);
				cmd.Parameters.AddWithValue("@companyId", companyId);

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

		[HttpPost]
		[Route("/api/Squad/{squadId}/CompanyRemove")]
		public IActionResult RemoveSquadFromCompany(int squadId)
		{
			using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand("UPDATE squad SET " +
					"companyid = NULL " +
					"WHERE id = @squadId; ", conn);
				cmd.Parameters.AddWithValue("@squadId", squadId);

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
