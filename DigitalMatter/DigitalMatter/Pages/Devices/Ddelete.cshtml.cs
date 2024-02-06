using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DigitalMatter.Pages.Devices
{
    public class DdeleteModel : PageModel
    {
        private readonly string connectionString;
        public string delete_id = "";
        public string errorMessage = "";

        public DdeleteModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        public void OnGet()
        {
            try
            {
                delete_id = Request.Query["id"];
                
            }

            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }


        }
        public void OnPost()
        {
            delete_id = Request.Query["id"];
            try
            {
                // string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM devicedb WHERE device_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", delete_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }


            delete_id = "";
            errorMessage = "";
            Response.Redirect("/Devices");
        }
    }
}
