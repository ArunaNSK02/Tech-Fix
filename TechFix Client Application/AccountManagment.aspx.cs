using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechFix_Client_Application
{
    
    public partial class AccountManagment : System.Web.UI.Page
    {
        private AuthenticationService.AuthenticationSoapClient WebServices = new AuthenticationService.AuthenticationSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    string username = Session["Username"] != null ? Session["Username"].ToString() : "User";
                    profileBtn.InnerText = $"Hi! {username}";
                    btnSubmit.Enabled = false;
                    btnSubmit.Style["opacity"] = "0.5";
                    btnSubmit.Style["cursor"] = "default";
                    lblName.Text = Session["Name"].ToString();
                    lblUsername.Text = username;
                    txtAddress.Attributes["placeholder"] = Session["Address"].ToString();
                    txtEmail.Attributes["placeholder"] = Session["Email"].ToString();
                    txtPhoneNumber.Attributes["placeholder"] = Session["PhoneNumber"].ToString();
                    lblRole.Text = Session["Role"].ToString();
                    lbljoinDate.Text = Session["CreatedAt"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            int userId = (int)Session["UserId"];
            List<string> fieldsToUpdate = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            bool isValid = true;

            

            if (!string.IsNullOrEmpty(txtEmail.Text) && isValid)
            {
                if (WebServices.CheckEmailFormat(txtEmail.Text))
                {
                    if(!WebServices.CheckEmailExists(txtEmail.Text))
                    {
                        fieldsToUpdate.Add("Email = @Email");
                        parameters.Add(new SqlParameter("@Email", txtEmail.Text));
                    } else
                    {
                        lblErrorMessage.Style["visibility"] = "visible";
                        lblErrorMessage.Text = "*Email already exsist!*";
                        isValid = false;
                    }

                } else
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Text = "*Invalid email format!*";
                    isValid = false;
                }

            }

            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                if(WebServices.CheckPasswordFormat(txtPassword.Text))
                {
                    if(!WebServices.CheckPasswordExists(txtPassword.Text))
                    {
                        fieldsToUpdate.Add("Password = @Password");
                        parameters.Add(new SqlParameter("@Password", txtPassword.Text));
                    } else
                    {
                        lblErrorMessage.Style["visibility"] = "visible";
                        lblErrorMessage.Text = "*Password already exsist!*";
                        isValid = false;
                    }

                } else
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Text = "*Password should contain uppercase, lowercase letters and numbers. Must be more than 6 characters long!*";
                    isValid = false;
                }
                
            }

            if (!string.IsNullOrEmpty(txtAddress.Text) && isValid)
            {
                fieldsToUpdate.Add("Address = @Address");
                parameters.Add(new SqlParameter("@Address", txtAddress.Text));
            }

            if (!string.IsNullOrEmpty(txtPhoneNumber.Text) && isValid)
            {
                fieldsToUpdate.Add("PhoneNumber = @PhoneNumber");
                parameters.Add(new SqlParameter("@PhoneNumber", txtPhoneNumber.Text));
            }

            // Construct and execute the query if there are fields to update
            if(isValid)
            {
                string query = $"UPDATE Users SET {string.Join(", ", fieldsToUpdate)} WHERE UserID = @userId";
                parameters.Add(new SqlParameter("@UserID", userId));

                string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Style["color"] = "green";
                lblErrorMessage.Text = "User information updated successfully.";
            }
            
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }
    }
}