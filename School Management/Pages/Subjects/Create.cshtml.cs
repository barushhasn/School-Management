using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Clients;
using System.Data.SqlClient;

namespace School_Management.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        public SubjectInfo subjectInfo = new SubjectInfo();
        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            subjectInfo.Name = Request.Form["name"];
            if (int.TryParse(Request.Form["lessons"], out int lessons))
            {
                subjectInfo.Lessons = lessons;
            }
            else
            {
                ModelState.AddModelError("Age", "Please enter a valid numeric age.");
            }

            if (subjectInfo.Name.Length == 0 || subjectInfo.Lessons.ToString().Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //Save the new Subject into the database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Subjects " +
                                 "(Name, Lessons) VALUES " +
                                 "(@Name, @Lessons)";
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

            subjectInfo.Name = "";
            subjectInfo.Lessons = 0;

            succesMessage = "New Subject Added Correctly";

            Response.Redirect("/subjects/Index");
        }
    }
}
