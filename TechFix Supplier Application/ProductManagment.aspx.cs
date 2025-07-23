using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechFix_Supplier_Application
{
    public partial class ProductManagment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                string name = Session["Name"] != null ? Session["Name"].ToString() : "User";
                profileBtn.InnerText = $"Hi! {name}";
                lblErrorMessage.Style["visibility"] = "hidden";
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        private string SaveFile(FileUpload fileUpload)
        {
            if (fileUpload.HasFile)
            {

                string sharedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Shared\Images\");
                // Ensure the directory exists, create it if it doesn't
                if (!Directory.Exists(sharedFolderPath))
                {
                    Directory.CreateDirectory(sharedFolderPath);  // Create the folder if it doesn't exist
                }

                // Sanitize file name
                string fileName = Path.GetFileName(fileUpload.FileName);  // Ensures only file name is used
                string safeFileName = Path.Combine(sharedFolderPath, fileName);

                // Save the file
                fileUpload.SaveAs(safeFileName);
                return fileName;  // Return relative path
            }
            return null;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Validation for quotation title and file upload
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Product Name must be filled!";
                isValid = false;
            } 
            else if(string.IsNullOrEmpty(txtDescription.Text))
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Product Description must be filled!";
                isValid = false;
            }
            else if (!fileUploadBtn.HasFile)
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Product Image must be attached!";
                isValid = false;
            }

            // Proceed if all validations pass
            if (isValid)
            {
                ProductService.ProductServiceSoapClient client = new ProductService.ProductServiceSoapClient();

                // Get form data
                string title = txtProductName.Text;
                string description = txtDescription.Text;
                string filePath = SaveFile(fileUploadBtn);
                int supplierID = (int)Session["UserId"];

                bool isUpdated = false;

                if(client.InsertProduct(title, description, filePath, supplierID) > 0)
                {
                    isUpdated = true;
                }

                // Handle message for success or failure
                if (isUpdated)
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Style["color"] = "green";
                    lblErrorMessage.Text = "New Product listed successfully!";
                }
                else
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Text = "Failed to list the product!";
                }
            }
        }
    }
}