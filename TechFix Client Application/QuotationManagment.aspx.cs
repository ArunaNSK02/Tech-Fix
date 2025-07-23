using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFix_Supplier_Application.QuotationServiceMod;

namespace TechFix_Client_Application
{
    public partial class QuotationManagment : System.Web.UI.Page
    {
        private string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {

                overlay.Style["visibility"] = "hidden";
                popup.Style["visibility"] = "hidden";

                

                string username = Session["Username"] != null ? Session["Username"].ToString() : "User";
                profileBtn.InnerText = $"Hi! {username}";
                lblErrorMessage.Style["visibility"] = "hidden";

                loadQuotationData();


                if (!IsPostBack)
                {
                    PopulateSuppliers();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void quotationsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Cast data item to Quotation type
                var quotation = (TechFix_Supplier_Application.QuotationServiceMod.Quotation)e.Item.DataItem;

                // Access the Status property
                string status = quotation.Status;

                // Find controls in the Repeater item
                LinkButton deleteLink = (LinkButton)e.Item.FindControl("deleteLink");
                LinkButton editLink = (LinkButton)e.Item.FindControl("editLink");
                Label respondedLabel = (Label)e.Item.FindControl("respondedLabel");

                // Show or hide controls based on the Status
                if (status == "pending")
                {
                    deleteLink.Visible = true;
                    editLink.Visible = true;
                    respondedLabel.Visible = false;
                }
                else if (status == "Accepted")
                {
                    deleteLink.Visible = false;
                    editLink.Visible = false;
                    respondedLabel.Visible = true;
                }
            }
        }



        private void loadQuotationData()
        {
            QuotationServiceMod.QuotationServiceSoapClient client = new QuotationServiceMod.QuotationServiceSoapClient();

            List<Quotation> quotations = client.GetAllQuotations().ToList();

            if (quotations.Count > 0)
            {
                quotationsRepeater.Visible = true;
                quotationsRepeater.DataSource = quotations;
                quotationsRepeater.DataBind();
            }
            else
            {
                quotationsRepeater.Visible = false;
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton deleteButton = (LinkButton)sender;
            int quotationId = int.Parse(deleteButton.CommandArgument);
            string query = "DELETE FROM Quotations WHERE QuotationID = @QuotationID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the parameter to the SQL command
                    cmd.Parameters.AddWithValue("@QuotationID", quotationId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            loadQuotationData();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
             overlay.Style["visibility"] = "visible";
             popup.Style["visibility"] = "visible";

            LinkButton editButton = (LinkButton)sender;
            int quotationId = int.Parse(editButton.CommandArgument);


            string title = string.Empty;
            string description = string.Empty;
            Session["quotationID"] = quotationId;

            string query = "SELECT Title, Note FROM Quotations WHERE QuotationID = @QuotationID";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@QuotationID", quotationId);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            title = reader["Title"].ToString();
                            if(string.IsNullOrEmpty(reader["Note"].ToString()))
                            {
                                description = "Not provided...";
                            } else
                            {
                                description = reader["Note"].ToString();
                            }

                        }
                    }
                }
            }


            updatetxtQuotationTitle.Attributes["placeholder"] = title;
            updatetxtDescription.Attributes["placeholder"] = description;

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            overlay.Style["visibility"] = "hidden";
            popup.Style["visibility"] = "hidden";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            int quotationId = (int)Session["quotationID"];

            List<string> fieldsToUpdate = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            bool needtoUpdate = false;



            if (!string.IsNullOrEmpty(updatetxtQuotationTitle.Text))
            {

                fieldsToUpdate.Add("Title = @Title");
                parameters.Add(new SqlParameter("@Title", updatetxtQuotationTitle.Text));
                needtoUpdate = true;

            }

            if (!string.IsNullOrEmpty(updatetxtDescription.Text))
            {
                fieldsToUpdate.Add("Note = @Note");
                parameters.Add(new SqlParameter("@Note", updatetxtDescription.Text));
                needtoUpdate = true;

            }

            if (updatefileUpload.HasFile)
            {
                string filePath = SaveFile(updatefileUpload);
                fieldsToUpdate.Add("Attachment = @Attachment");
                parameters.Add(new SqlParameter("@Attachment", filePath));
                needtoUpdate = true;
            }

            if (needtoUpdate)
            {
                string query = $"UPDATE Quotations SET {string.Join(", ", fieldsToUpdate)} WHERE QuotationID = @quotationId";
                parameters.Add(new SqlParameter("@quotationId", quotationId));

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                loadQuotationData();
            }
        }




        private void PopulateSuppliers()
        {
            // Assuming you have a Suppliers table with SupplierID and SupplierName columns
            string query = "SELECT SupplierID, SupplierName FROM Suppliers";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                supplierDL.DataSource = reader;
                supplierDL.DataTextField = "SupplierName";
                supplierDL.DataValueField = "SupplierID";
                supplierDL.DataBind();
            }
            supplierDL.Items.Insert(0, new ListItem("All Suppliers", "0"));
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Validation for quotation title and file upload
            if (string.IsNullOrEmpty(txtQuotationTitle.Text))
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Quotation title must be filled!";
                isValid = false;
            }
            else if (!fileUploadBtn.HasFile)
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Quotation file must be attached!";
                isValid = false;
            }

            // Proceed if all validations pass
            if (isValid)
            {
                QuotationServiceMod.QuotationServiceSoapClient client = new QuotationServiceMod.QuotationServiceSoapClient();

                // Get form data
                string title = txtQuotationTitle.Text;
                string description = txtDescription.Text;
                string filePath = SaveFile(fileUploadBtn);

                // Get the selected supplier(s)
                int[] selectedSuppliers = GetSelectedSuppliers();
                bool isUpdated = false;

                // Handle insertion based on the selected suppliers
                if (selectedSuppliers.Length > 0)
                {
                    for(int index = 0; index < selectedSuppliers.Length; index++)
                    {
                        if(client.InsertQuotation(title, description, filePath, selectedSuppliers[index]) > 0)
                        {
                            isUpdated = true;
                        }
                    }
                }

                // Handle message for success or failure
                if (isUpdated)
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Style["color"] = "green";
                    lblErrorMessage.Text = "Quotation sent successfully!";
                    loadQuotationData();
                }
                else
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Text = "Failed to send the Quotation!";
                }
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


        private int[] GetSelectedSuppliers()
        {
            List<int> supplierIds = new List<int>();

            // Check if "All Suppliers" is selected
            if (supplierDL.SelectedValue == "0")
            {
                // Retrieve all supplier IDs if "All Suppliers" is selected
                supplierIds = GetAllSupplierIds();
            }
            else
            {
                // Retrieve only the selected supplier’s ID
                supplierIds.Add(int.Parse(supplierDL.SelectedItem.Value));
            }

            return supplierIds.ToArray();
        }



        private List<int> GetAllSupplierIds()
        {
            List<int> supplierIds = new List<int>();

            string query = "SELECT SupplierID FROM Suppliers"; 
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    supplierIds.Add(reader.GetInt32(0));
                }
            }
            return supplierIds;
        }
    }
}
