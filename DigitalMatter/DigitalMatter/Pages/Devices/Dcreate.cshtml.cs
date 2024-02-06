using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DigitalMatter.Pages.Devices
{
    public class DcreateModel : PageModel
    {
        private readonly string connectionString;
        public DeviceDb dev = new DeviceDb();
        public string errorMessage = "";
        public string success = "";
        public DcreateModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {

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
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

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
                                errorMessage = "The group id has to be one that already exists and not NULL";
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
                    string sql = "INSERT INTO devicedb " +
                        "(device_name,group_id) " +
                        "VALUES (@name,@id);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", dev.device_name);
                        command.Parameters.AddWithValue("@id", dev.group_id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            dev.device_name = "";
            dev.group_id = "";
            errorMessage = "";
            success = "New group added";
        }
    }
}
