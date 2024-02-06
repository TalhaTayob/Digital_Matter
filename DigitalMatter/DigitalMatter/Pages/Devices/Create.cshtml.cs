using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DigitalMatter.Pages.Devices
{
    public class CreateModel : PageModel
    {
        private readonly string connectionString;
        public GroupDb group = new GroupDb();
        public string errorMessage = "";
        public string success = "";
        public CreateModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            //group.id = Request.Form["groupID"];
            group.name = Request.Form["groupname"];
            group.parent_id = Request.Form["parentID"];
            int x = 0;
            try
            {
                x = Int32.Parse(group.parent_id);
            }
            
            catch (Exception ex)
            {
                errorMessage="Please enter an integer for parent_id";
                return;
            }


            if (group.name.Length == 0 || group.parent_id.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }


            try
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT group_id FROM groupdb WHERE group_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", group.parent_id);                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                errorMessage = "The parent_id has to be one that already exists and not NULL";
                                return;
                            }                          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }



            try 
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO groupdb " +
                        "(group_name,parent_id) " +
                        "VALUES (@name,@id);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", group.name);
                        command.Parameters.AddWithValue("@id", group.parent_id);
                        command.ExecuteNonQuery();
                    }
                }
            } 
            catch(Exception ex)
            {
                errorMessage=ex.Message;
                return;
            }
            
            
            group.name = "";
            group.parent_id = "";
            success = "New group added";
        }
    }
}
