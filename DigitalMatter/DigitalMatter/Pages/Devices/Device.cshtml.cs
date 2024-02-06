using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DigitalMatter.Pages.Devices
{
    public class DeviceModel : PageModel
    {
        private readonly string connectionString;
        public List<DeviceDb> dev = new List<DeviceDb>();
        public List<string> g=new List<string>();

        public DeviceModel(IConfiguration configuration)
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

            foreach (string item in g)
            {
                try
                {
                    //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM devicedb WHERE group_id=@id ORDER BY device_id;";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", item);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DeviceDb model = new DeviceDb();
                                    model.device_id = reader.GetInt32(0).ToString();
                                    model.group_id = reader.GetInt32(1).ToString();
                                    model.device_name = reader.GetString(2);
                                    model.firmware = reader.GetDouble(3).ToString();
                                    dev.Add(model);
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
        }

    }
    public class DeviceDb
    {
        public string? device_id;
        public string? group_id;
        public string? device_name;
        public string? firmware;
    }

}
