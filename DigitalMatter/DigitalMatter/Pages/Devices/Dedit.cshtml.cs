using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DigitalMatter.Pages.Devices
{
    public class DeditModel : PageModel
    {
        public DeviceDb dev = new DeviceDb();
        public string errorMessage = "";
        public string success = "";
        public List<string> g = new List<string>();
        private readonly string connectionString;

        public DeditModel(IConfiguration configuration)
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
                    string sql = "SELECT * FROM devicedb where device_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dev.device_id = reader.GetInt32(0).ToString();
                                dev.group_id = reader.GetInt32(1).ToString();
                                dev.device_name = reader.GetString(2);
                                dev.firmware = reader.GetDouble(3).ToString();
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
            dev.device_name = Request.Form["devname"];
            dev.group_id = Request.Form["GID"];
            int x = 0;
            try
            {
                x = Int32.Parse(dev.group_id);
            }

            catch (Exception ex)
            {
                errorMessage = "Please enter an integer for parent_id";
                return;
            }


            if (dev.device_name.Length == 0)
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
                        command.Parameters.AddWithValue("@id", dev.group_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                errorMessage = "The associated id has to be of a group that already exists and not NULL";
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
                    string sql = "UPDATE devicedb SET device_name=@name, group_id=@pid WHERE device_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@pid", dev.group_id);
                        command.Parameters.AddWithValue("@name", dev.device_name);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            success = "Successfully edited device";
            errorMessage = "";
        }
    }
}

