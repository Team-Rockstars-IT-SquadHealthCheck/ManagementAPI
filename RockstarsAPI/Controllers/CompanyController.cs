using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RockstarsAPI.models;
using System.Data.SqlClient;
using System.Data;

namespace RockstarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        public CompanyController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [HttpGet]
        [Route("/Companies")]
        public List<Company> AllCompanies()
        {
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM company", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            List<Company> companyList = new List<Company>();
            if (datatableuser.Rows.Count > 0)
            {
                for (int i = 0; i < datatableuser.Rows.Count; i++)
                {
                    Company company = new Company();
                    company.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
                    company.Name = Convert.ToString(datatableuser.Rows[i]["name"]);
                    company.Adress = Convert.ToString(datatableuser.Rows[i]["address"]);
                    company.Telephonenr = Convert.ToString(datatableuser.Rows[i]["telephonenr"]);
                    companyList.Add(company);

                }
            }
            return companyList;
        }

        [HttpGet]
        [Route("/CompanyDetails/{id}")]
        public Company Details(int? id)
        {
            Company company = new Company();
            company = CompanyInfo(id);

            return company;
        }

        [HttpGet]
        private Company CompanyInfo(int? id)
        {
            Company company = new Company();
            HttpContext.Response.Headers.Add("Content-Type", "application/json");
            HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
            SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM company WHERE id = {id}", conn);
            DataTable datatableuser = new DataTable();
            adapter.Fill(datatableuser);
            if (datatableuser.Rows.Count > 0)
            {
                company.Id = Convert.ToInt32(datatableuser.Rows[0]["id"]);
                company.Name = Convert.ToString(datatableuser.Rows[0]["name"]);
                company.Adress = Convert.ToString(datatableuser.Rows[0]["address"]);
                company.Telephonenr = Convert.ToString(datatableuser.Rows[0]["telephonenr"]);


            }
            return company;
        }

		[HttpGet]
		[Route("/CompanyName")]
		public List<Company> GetCompanyByName(string? name)
		{
			List<Company> companies = new List<Company>();
			HttpContext.Response.Headers.Add("Content-Type", "application/json");
			HttpContext.Response.Headers.Add("vary", "Accept-Encoding");
			SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString());
			SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM company WHERE name LIKE '%{name}%'", conn);
			DataTable datatableuser = new DataTable();
			adapter.Fill(datatableuser);
			if (datatableuser.Rows.Count > 0)
			{
				for (int i = 0; i < datatableuser.Rows.Count; i++)
				{
					Company company = new Company();
					company.Id = Convert.ToInt32(datatableuser.Rows[i]["id"]);
					company.Name = Convert.ToString(datatableuser.Rows[i]["name"]);
					company.Adress = Convert.ToString(datatableuser.Rows[i]["address"]);
					company.Telephonenr = Convert.ToString(datatableuser.Rows[i]["telephonenr"]);
					companies.Add(company);

				}


			}
			return companies;
		}

		[HttpPost]
        [Route("/Company")]
        public IActionResult CreateNewCompany([FromBody] Company company)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO company (name, address, telephonenr) VALUES (@name, @address, @telephonenr)", conn);
                cmd.Parameters.AddWithValue("@name", company.Name);
                cmd.Parameters.AddWithValue("@address", company.Adress);
                cmd.Parameters.AddWithValue("@telephonenr", company.Telephonenr);

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
        [Route("/Company/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE company WHERE company.id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
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
