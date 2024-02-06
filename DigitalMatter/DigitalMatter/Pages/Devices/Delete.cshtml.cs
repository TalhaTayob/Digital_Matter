using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DigitalMatter.Pages.Devices
{
    public class DeleteModel : PageModel
    {
        private readonly string connectionString;
        public string delete_id="";
        public string parent_id ="";
        public string errorMessage ="";

        public DeleteModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        public void OnGet()
        {
            try
            {
                delete_id = Request.Query["id"];
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT parent_id FROM groupdb WHERE group_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", delete_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {                               
                                if (reader.IsDBNull(0))
                                {
                                    errorMessage = "You cannot delete the root node";
                                    return;
                                }
                                else
                                {
                                    parent_id = reader.GetInt32(0).ToString();
                                    Console.WriteLine(parent_id);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }

            
        }
        public void OnPost() 
        {

            try
            {
                delete_id = Request.Query["id"];
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT parent_id FROM groupdb WHERE group_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", delete_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0))
                                {
                                    errorMessage = "You cannot delete the root node";
                                    return;
                                }
                                else
                                {
                                    parent_id = reader.GetInt32(0).ToString();
                                    Console.WriteLine(parent_id);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }

            try
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE devicedb SET group_id =@p_id WHERE group_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", delete_id);
                        command.Parameters.AddWithValue("@p_id", parent_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }
            
            try
            {
               // string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE groupdb SET parent_id =@parent_id WHERE parent_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", delete_id);
                        command.Parameters.AddWithValue("@parent_id", parent_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return;
            }

            try
            {
               // string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM groupdb WHERE group_id=@id;";
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
            parent_id = "";
            errorMessage = "";
            Response.Redirect("/Devices");
        }   
    }
}
