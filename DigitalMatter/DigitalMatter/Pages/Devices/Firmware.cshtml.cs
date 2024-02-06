using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DigitalMatter.Pages.Devices
{
    public class FirmwareModel : PageModel
    {
        private readonly string connectionString;

        public List<string> fware = new List<string>();
        public string cfirmware { get; set; } = "";
        public FirmwareModel(IConfiguration configuration)
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
                    string sql = "SELECT firmware FROM Firmware WHERE device_id=@id ORDER BY firmware;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fware.Add(reader.GetDouble(0).ToString());
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
                    string sql = "SELECT firmware FROM devicedb WHERE device_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cfirmware=reader.GetDouble(0).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost()
        {
            string selectedfirmware = Request.Form["firmware_version"];
            selectedfirmware = selectedfirmware.Replace(',', '.');
            string id = Request.Query["id"];
            try
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE devicedb " +
                                 "SET firmware=@firmware " +
                                 "WHERE device_id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firmware", selectedfirmware);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            selectedfirmware = "";
            Response.Redirect("/Devices");
        }
    }
    /*public class firmwareDb
    {
        public string? ID;
        public string? device_id;
        public string? firmware;
    }*/
}
