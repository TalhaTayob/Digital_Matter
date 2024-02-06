using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;


namespace DigitalMatter.Pages.Devices
{
    public class IndexModel : PageModel
    {

        private readonly string connectionString;

        public List<GroupDb> groups= new List<GroupDb>();

        public IndexModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }
        public void OnGet()
        {
            Console.WriteLine(connectionString);

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM groupdb";
                    using (SqlCommand command = new SqlCommand(sql,connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        { 
                            while (reader.Read())
                            {
                                GroupDb model = new GroupDb();
                                model.id = reader.GetInt32(0).ToString();
                                model.name= reader.GetString(1);
                                if (!reader.IsDBNull(2))
                                {
                                    model.parent_id = reader.GetInt32(2).ToString();
                                }
                                else
                                {
                                    model.parent_id = "NULL";
                                }
                                groups.Add(model);
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
    public class GroupDb 
    {
        public string? id;
        public string? name;
        public string? parent_id;
        //public string? firmware;
    }
}
