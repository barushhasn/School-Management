using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Subjects;
using System.Data.SqlClient;

namespace School_Management.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        public List<TeacherInfo> teacherInfos = new List<TeacherInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Teachers";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TeacherInfo teacherInfo = new TeacherInfo();

                                teacherInfo.Id = reader.GetInt32(0);
                                teacherInfo.FirstName = reader.GetString(1);
                                teacherInfo.LastName = reader.GetString(2);
                                teacherInfo.Address = reader.GetString(3);
                                teacherInfo.Phone = reader.GetString(4);
                                teacherInfo.SubjectId = reader.GetInt32(5);

                                teacherInfos.Add(teacherInfo);
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

    public class TeacherInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int SubjectId { get; set; }
    }
}
