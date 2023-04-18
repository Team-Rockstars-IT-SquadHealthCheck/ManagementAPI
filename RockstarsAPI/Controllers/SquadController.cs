﻿using Microsoft.AspNetCore.Http;
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
                    squad.CompanyId = Convert.ToInt32(datatableuser.Rows[i]["companyid"]);
                    squad.CompanyName = Convert.ToString(datatableuser.Rows[i]["companyname"]);
                    
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
                squad.CompanyId = Convert.ToInt32(datatableuser.Rows[0]["companyid"]);
                squad.CompanyName = Convert.ToString(datatableuser.Rows[0]["companyname"]);
                

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
                    squad.CompanyId = Convert.ToInt32(datatableuser.Rows[i]["companyid"]);
                    squad.CompanyName = Convert.ToString(datatableuser.Rows[i]["companyname"]);
                    

                    squads.Add(squad);
                }
            }

            return squads;
        }

        [HttpPost]
        public IActionResult NewSquad([FromBody]Squad squad)
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

    }
}
