using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School_Management.Pages.Clients;
using System.Data.SqlClient;

namespace School_Management.Pages.Students
{
    public class EditModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public String errorMessage = "";
        public String succesMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=School;Integrated Security=True";

                using (SqlConnection connection  = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Students WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentInfo.Id = reader.GetInt32(0);
                                studentInfo.FirstName = reader.GetString(1);
                                studentInfo.MiddleName = reader.GetString(2);
                                studentInfo.LastName = reader.GetString(3);
                                studentInfo.Age = reader.GetInt32(4);
                                studentInfo.Address = reader.GetString(5);
                                studentInfo.Phone = reader.GetString(6);
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
                studentInfo.Id = id;
            }
            else
            {
                ModelState.AddModelError("Id", "Please enter a valid numeric ID.");
            }
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

            if (studentInfo.Id.ToString().Length == 0 || studentInfo.FirstName.Length == 0 || 
                studentInfo.MiddleName.Length == 0 || studentInfo.LastName.Length == 0 ||
                studentInfo.Age.ToString().Length == 0 || studentInfo.Address.Length == 0 || 
                studentInfo.Phone.Length == 0)
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
                    String sql = "UPDATE Students " +
                                 "SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Age = @Age, Address = @Address, Phone = @Phone " +
                                 "WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", studentInfo.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", studentInfo.MiddleName);
                        command.Parameters.AddWithValue("@LastName", studentInfo.LastName);
                        command.Parameters.AddWithValue("@Age", studentInfo.Age);
                        command.Parameters.AddWithValue("@Address", studentInfo.Address);
                        command.Parameters.AddWithValue("@Phone", studentInfo.Phone);
                        command.Parameters.AddWithValue("@Id", studentInfo.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Students/Index");
        }
    }
}
