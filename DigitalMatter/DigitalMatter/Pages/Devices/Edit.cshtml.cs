using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DigitalMatter.Pages.Devices
{
    public class EditModel : PageModel
    {
        public GroupDb group = new GroupDb();
        public string errorMessage = "";
        public string success = "";
        public List<string> g = new List<string>();
        private readonly string connectionString;

        public EditModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        public void OnGet()
        {
            string id = Request.Query["id"];

            try 
            {

                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM groupdb where group_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {                              
                                group.id = reader.GetInt32(0).ToString();
                                group.name = reader.GetString(1);
                                if (!reader.IsDBNull(2))
                                {
                                    group.parent_id = reader.GetInt32(2).ToString();
                                }
                                else
                                {
                                    group.parent_id = "NULL";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            { 
                errorMessage = ex.Message;
            }
        }
        public void OnPost() 
        {
            string id = Request.Query["id"];
            group.name = Request.Form["groupname"];
            group.parent_id = Request.Form["parentID"];
            int x = 0;
            try
            {
                x = Int32.Parse(group.parent_id);
            }

            catch (Exception ex)
            {
                errorMessage = "Please enter an integer for parent_id";
                return;
            }


            if (group.name.Length == 0 || group.parent_id.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
               // string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

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

                    string sql = "WITH CTE as (SELECT group_id, group_name, parent_id FROM groupdb WHERE group_id =@id UNION ALL SELECT e.group_id, e.group_name, e.parent_id FROM groupdb e INNER JOIN CTE o ON o.group_id = e.parent_id ) SELECT group_id FROM CTE";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                g.Add(reader.GetInt32(0).ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            if (g.Contains(group.parent_id))
            {
                errorMessage = "The input parent_id either hasn't changed or contains the ID of a child in the selected branch";
            }

            try 
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                connection.Open ();
                    string sql = "UPDATE groupdb SET group_name=@name, parent_id=@pid WHERE group_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@pid", group.parent_id);
                        command.Parameters.AddWithValue("@name", group.name);
                        command.ExecuteNonQuery();
                    }
                }
            } 
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
    }
}
