using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace School_Management.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        public List<SubjectInfo> subjectInfos = new List<SubjectInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Subjects";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SubjectInfo subjectInfo = new SubjectInfo();

                                subjectInfo.Id = reader.GetInt32(0);
                                subjectInfo.Name = reader.GetString(1);
                                subjectInfo.Lessons = reader.GetInt32(2);

                                subjectInfos.Add(subjectInfo);
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

    public class SubjectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Lessons { get; set; }
    }
}
