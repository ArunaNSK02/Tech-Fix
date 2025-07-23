using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFix_Supplier_Application.QuotationServiceMod;

namespace TechFix_Supplier_Application
{
    
    public partial class QuotationManagment : System.Web.UI.Page
    {
        private string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    string name = Session["Name"] != null ? Session["Name"].ToString() : "User";
                    profileBtn.InnerText = $"Hi! {name}";

                    GetAllQuotaions();
                    lblOrderMessage.Visible = false;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void GetAllQuotaions()
        {
            int supplierID = (int)Session["UserId"];
            QuotationServiceMod.QuotationServiceSoapClient client = new QuotationServiceMod.QuotationServiceSoapClient();

            List<Quotation> quotations = client.GetAllQuotationsBySupplier(supplierID).ToList();

            if (quotations.Count == 0)
            {
                QuotationRepeater.Visible = false;
                lblOrderMessage.Text = "No Quotations Found!";
                lblOrderMessage.Visible = true;
            }
            else
            {
                QuotationRepeater.DataSource = quotations;
                QuotationRepeater.DataBind();
                QuotationRepeater.Visible = true;
            }
        }

        private string SaveFile(FileUpload fileUpload)
        {
            if (fileUpload.HasFile)
            {

                string sharedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Shared\Quotation\");
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            Button btnSubmit = (Button)sender;
            int QuotationyID = Convert.ToInt32(btnSubmit.CommandArgument);
            // Get the RepeaterItem that contains this Button
            RepeaterItem item = (RepeaterItem)btnSubmit.NamingContainer;

            // Find the FileUpload control within this RepeaterItem
            FileUpload fileUpload = (FileUpload)item.FindControl("btnResponseFileUpload");

            if (fileUpload != null && fileUpload.HasFile)
            {
                // Process the uploaded file
                string filePath = SaveFile(fileUpload);

                string updateQuery = "UPDATE Quotations SET SupplierResponse = @filePath, Status = @status WHERE QuotationID = @QuotationID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand updateInventoryCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateInventoryCmd.Parameters.AddWithValue("@filePath", filePath);
                        updateInventoryCmd.Parameters.AddWithValue("@status", "Accepted");
                        updateInventoryCmd.Parameters.AddWithValue("@QuotationID", QuotationyID);
                        updateInventoryCmd.ExecuteNonQuery();
                    }
                }

                GetAllQuotaions();


                // You can now save the file path to the database or perform other actions as needed
            }
            else
            {
                // Handle the case where no file was uploaded
                //lblMessage.Text = "Please select a file to upload.";
            }
        }
    }
}