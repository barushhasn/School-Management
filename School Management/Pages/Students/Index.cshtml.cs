using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace School_Management.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<StudentInfo> studentInfos = new List<StudentInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM STUDENTS";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentInfo studentInfo = new StudentInfo();

                                studentInfo.Id = reader.GetInt32(0);
                                studentInfo.FirstName = reader.GetString(1);
                                studentInfo.MiddleName = reader.GetString(2);
                                studentInfo.LastName = reader.GetString(3);
                                studentInfo.Age = reader.GetInt32(4);
                                studentInfo.Address = reader.GetString(5);
                                studentInfo.Phone = reader.GetString(6);

                                studentInfos.Add(studentInfo);
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

    public class StudentInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
