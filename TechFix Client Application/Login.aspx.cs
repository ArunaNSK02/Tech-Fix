using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechFix_Client_Application
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblErrorMessage.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Check if username or password fields are empty
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblErrorMessage.Text = "*Username or Password fields can not be blancked!*";
                lblErrorMessage.Visible = true; 
                return;
            }

            AuthenticationService.AuthenticationSoapClient authService = new AuthenticationService.AuthenticationSoapClient();

            AuthenticationService.AuthenticationResponse response = authService.AuthenticateUser(txtUsername.Text, txtPassword.Text);

            if (response.IsAuthenticated)
            {
                // Store user information in session
                Session["UserId"] = response.UserId;
                Session["Username"] = txtUsername.Text;
                Session["Password"] = txtPassword.Text;
                Session["Role"] = response.Role;
                Session["CreatedAt"] = response.CreatedAt;
                Session["Name"] = response.Name;
                Session["Address"] = response.Address;
                Session["PhoneNumber"] = response.PhoneNumber;
                Session["Email"] = response.Email;

                switch (response.Role)
                {
                    case "Admin":
                        Response.Redirect("User Managment.aspx");
                        break;
                    case "ProcurementOfficer":
                        Response.Redirect("StaffDashboard.aspx");
                        break;
                    default:
                        lblErrorMessage.Text = "*Unrecognized user role. Access Denied!*";
                        break;
                }
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "*Invalid Username or Password!*";
            }
        }
    }
}