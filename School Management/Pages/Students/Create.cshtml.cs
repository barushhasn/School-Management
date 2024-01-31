using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Clients;
using System.Data.SqlClient;

namespace School_Management.Pages.Students
{
    public class CreateModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        {
            studentInfo.FirstName = Request.Form["firstName"];
            studentInfo.MiddleName = Request.Form["middleName"];
            studentInfo.LastName = Request.Form["lastName"];
            if (int.TryParse(Request.Form["age"], out int age))
            {
                studentInfo.Age = age;
            }
            else
            {
                ModelState.AddModelError("Age", "Please enter a valid numeric age.");
            }
            studentInfo.Address = Request.Form["address"];
            studentInfo.Phone = Request.Form["phone"];

            if (studentInfo.FirstName.Length == 0 || studentInfo.MiddleName.Length == 0 || studentInfo.LastName.Length == 0 || 
                studentInfo.Age.ToString().Length == 0 || studentInfo.Address.Length == 0 || studentInfo.Phone.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            // Save the new Student into the database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO STUDENTS " +
                                 "(FirstName, MiddleName, LastName, Age, Address, Phone) " +
                                 "VALUES (@FirstName, @MiddleName, @LastName, @Age, @Address, @Phone)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", studentInfo.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", studentInfo.MiddleName);
                        command.Parameters.AddWithValue("@LastName", studentInfo.LastName);
                        command.Parameters.AddWithValue("@Age", studentInfo.Age);
                        command.Parameters.AddWithValue("@Address", studentInfo.Address);
                        command.Parameters.AddWithValue("@Phone", studentInfo.Phone);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

            studentInfo.FirstName = "";
            studentInfo.MiddleName = "";
            studentInfo.LastName = "";
            studentInfo.Age = 0;
            studentInfo.Address = "";
            studentInfo.Phone = "";
            succesMessage = "New Student Added Succesfully";

            Response.Redirect("/Students/Index");
        }
    }
}
