using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Clients;
using School_Management.Pages.Subjects;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace School_Management.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        public TeacherInfo teacherInfo = new TeacherInfo();
        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
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

            //Save the new Teacher into the database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Teachers " +
                                 "(FirstName, LastName, Address, Phone, SubjectId) VALUES " +
                                 "(@FirstName, @LastName, @Address, @Phone, @SubjectId)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", teacherInfo.FirstName);
                        command.Parameters.AddWithValue("@LastName", teacherInfo.LastName);
                        command.Parameters.AddWithValue("@Address", teacherInfo.Address);
                        command.Parameters.AddWithValue("@Phone", teacherInfo.Phone);
                        command.Parameters.AddWithValue("@SubjectId", teacherInfo.SubjectId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            teacherInfo.FirstName = "";
            teacherInfo.LastName = "";
            teacherInfo.Address = "";
            teacherInfo.Phone = "";
            teacherInfo.SubjectId = 0;
            succesMessage = "New Teacher Added Correctly";

            Response.Redirect("/Teachers/Index");
        }
    }
}
