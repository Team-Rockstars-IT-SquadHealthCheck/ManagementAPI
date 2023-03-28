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
        public List<Company> GetAllCompanies()
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
        public Company GetDetails(int? id)
        {
            Company company = new Company();
            company = GetCompanyInfo(id);

            return company;
        }

        [HttpGet]
        private Company GetCompanyInfo(int? id)
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
                company.Adress = Convert.ToString(datatableuser.Rows[0]["adress"]);
                company.Telephonenr = Convert.ToString(datatableuser.Rows[0]["telephonenr"]);


            }
            return company;
        }
        [HttpPost]
        [Route("/Company")]
        public IActionResult CreateNewCompanyy([FromBody] Company company)
        {
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("SqlServer").ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO company (name, adress, telephonenr) VALUES (@name, @adress, @telephonenr)", conn);
                cmd.Parameters.AddWithValue("@name", company.Name);
                cmd.Parameters.AddWithValue("@adress", company.Adress);
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
    }
}
