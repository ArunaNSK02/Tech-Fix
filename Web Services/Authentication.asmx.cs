using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Web_Services
{
    /// <summary>
    /// Summary description for Authentication
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Authentication : System.Web.Services.WebService
    {
        private readonly string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";


        [WebMethod]
        public AuthenticationResponse AuthenticateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserID, Role, CreatedAt, Name, Address, PhoneNumber, Email FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AuthenticationResponse
                            {
                                IsAuthenticated = true,
                                UserId = reader.GetInt32(0),
                                Role = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss"),
                                Name = reader.GetString(3),
                                Address = reader.GetString(4),
                                PhoneNumber = reader.GetString(5),
                                Email = reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return new AuthenticationResponse { IsAuthenticated = false };
        }


        [WebMethod]
        public AuthenticationResponse AuthenticateSupplier(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SupplierID, CreatedAt, SupplierName, Address, ContactPhone, ContactEmail FROM Suppliers WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AuthenticationResponse
                            {
                                IsAuthenticated = true,
                                UserId = reader.GetInt32(0),
                                Role = "SupplierRepresentative",
                                CreatedAt = reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss"),
                                Name = reader.GetString(2),
                                Address = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Email = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return new AuthenticationResponse { IsAuthenticated = false };
        }

        [WebMethod]
        public bool CheckEmailExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();

                    // Return true if the count is greater than 0, meaning the email exists
                    return count > 0;
                }
            }

        }

        [WebMethod]
        public bool CheckPasswordExists(string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", password);
                    int count = (int)cmd.ExecuteScalar();

                    // Return true if the count is greater than 0, meaning the password exists
                    return count > 0;
                }
            }

        }

        [WebMethod]
        public bool CheckEmailFormat(string email)
        {
            // Basic email format pattern
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        [WebMethod]
        public bool CheckPasswordFormat(string password)
        {
            // Password pattern: at least one uppercase letter, one lowercase letter, one digit, minimum 6 characters
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            return Regex.IsMatch(password, passwordPattern);
        }

    }

    public class AuthenticationResponse 
    {
        public bool IsAuthenticated { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public string CreatedAt { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
