using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Subjects;
using System.Data.SqlClient;

namespace School_Management.Pages.Teachers
{
    public class EditModel : PageModel
    {
        public TeacherInfo teacherInfo = new TeacherInfo();

        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Teachers WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                teacherInfo.Id = reader.GetInt32(0);
                                teacherInfo.FirstName = reader.GetString(1);
                                teacherInfo.LastName = reader.GetString(2);
                                teacherInfo.Address = reader.GetString(3);
                                teacherInfo.Phone = reader.GetString(4);
                                teacherInfo.SubjectId = reader.GetInt32(5);
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
            if (int.TryParse(Request.Form["id"], out int id))
            {
                teacherInfo.Id = id;
            }
            teacherInfo.FirstName = Request.Form["firstName"];
            teacherInfo.LastName = Request.Form["lastName"];
            teacherInfo.Address = Request.Form["address"];
            teacherInfo.Phone = Request.Form["phone"];
            if (int.TryParse(Request.Form["subjectId"], out int subjectId))
            {
                teacherInfo.SubjectId = subjectId;
            }

            if (teacherInfo.FirstName.Length == 0 || teacherInfo.LastName.Length == 0 ||
                teacherInfo.Address.Length == 0 || teacherInfo.Phone.Length == 0 ||
                teacherInfo.SubjectId.ToString().Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Teachers " +
                                 "SET FirstName = @FirstName, LastName = @LastName, Address = @Address, Phone = @Phone, SubjectId = @SubjectId " +
                                 "WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", teacherInfo.FirstName);
                        command.Parameters.AddWithValue("@LastName", teacherInfo.LastName);
                        command.Parameters.AddWithValue("@Address", teacherInfo.Address);
                        command.Parameters.AddWithValue("@Phone", teacherInfo.Phone);
                        command.Parameters.AddWithValue("@SubjectId", teacherInfo.SubjectId);
                        command.Parameters.AddWithValue("@Id", teacherInfo.Id);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Teachers/Index");
        }
    }
}
