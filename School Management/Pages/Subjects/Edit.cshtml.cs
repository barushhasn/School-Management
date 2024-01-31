using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Clients;
using System.Data.SqlClient;

namespace School_Management.Pages.Subjects
{
    public class EditModel : PageModel
    {
        public SubjectInfo subjectInfo = new SubjectInfo();
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
                    String sql = "SELECT * FROM Subjects WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                subjectInfo.Id = reader.GetInt32(0);
                                subjectInfo.Name = reader.GetString(1);
                                subjectInfo.Lessons = reader.GetInt32(2);
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
                subjectInfo.Id = id;
            }
            else
            {
                ModelState.AddModelError("Id", "Please enter a valid numeric ID.");
            }
            subjectInfo.Name = Request.Form["name"];
            if (int.TryParse(Request.Form["lessons"], out int lessons))
            {
                subjectInfo.Id = lessons;
            }
            else
            {
                ModelState.AddModelError("Lessons", "Please enter a valid numeric Lesson.");
            }

            if (subjectInfo.Id.ToString().Length == 0 || subjectInfo.Name.Length == 0 || 
                subjectInfo.Lessons.ToString().Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Subjects " +
                                 "SET Name = @Name, Lessons = @Lessons " +
                                 "WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", subjectInfo.Name);
                        command.Parameters.AddWithValue("@Lessons", subjectInfo.Lessons);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Subjects/Index");
        }
    }
}
