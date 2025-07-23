using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFix_Supplier_Application.InventoryService;

namespace TechFix_Supplier_Application
{
    public partial class InventoryManagment : System.Web.UI.Page
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

                    loadInventoryData();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }

                PopulateProducts();
            }
        }

        protected void InventoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Access the Inventory item
                var dataItem = (TechFix_Supplier_Application.InventoryService.Inventory)e.Item.DataItem;
                string status = dataItem.Status;

                // Find controls in the Repeater
                LinkButton deleteLink = (LinkButton)e.Item.FindControl("deleteLink");
                LinkButton confirmLink = (LinkButton)e.Item.FindControl("confirmLink");
                Literal confirmedText = (Literal)e.Item.FindControl("confirmedText");

                // Show buttons or "Confirmed" text based on status
                if (status == "Pending")
                {
                    deleteLink.Visible = true;
                    confirmLink.Visible = true;
                }
                else
                {
                    confirmedText.Visible = true;
                }
            }
        }


        private void loadInventoryData()
        {
            int supplierID = (int)Session["UserId"];

            InventoryService.InventoryServiceSoapClient client = new InventoryService.InventoryServiceSoapClient();

            lblErrorMessage.Style["visibility"] = "hidden";

            List<Inventory> inventories = client.GetAllInventories(supplierID).ToList();

            if (inventories.Count == 0)
            {
                InventoryRepeater.Visible = false;  
            }
            else
            {
                InventoryRepeater.DataSource = inventories;
                InventoryRepeater.DataBind();
                InventoryRepeater.Visible = true;
            }
        }



        protected void DeleteInventory_Click(object sender, EventArgs e)
        {
            var button = (LinkButton)sender;
            int inventoryID = Convert.ToInt32(button.CommandArgument);

            string deleteQuery = "DELETE FROM Inventory WHERE InventoryID = @InventoryID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@InventoryID", inventoryID);
                    cmd.ExecuteNonQuery();
                }
            }

            loadInventoryData();
        }

        protected void ConfirmInventory_Click(object sender, EventArgs e)
        {
            var button = (LinkButton)sender;
            int inventoryID = Convert.ToInt32(button.CommandArgument);

            // Query to get StockQuantity and ProductID from the Inventory table
            string selectQuery = "SELECT StockQuantity, ProductID FROM Inventory WHERE InventoryID = @InventoryID";

            // Update query to confirm the inventory status
            string updateInventoryQuery = "UPDATE Inventory SET Status = 'Confirmed' WHERE InventoryID = @InventoryID";

            // Update query to add StockQuantity to the Quantity in Products table
            string updateProductQuantityQuery = "UPDATE Products SET Quantity = Quantity + @StockQuantity WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Retrieve StockQuantity and ProductID from Inventory
                int stockQuantity = 0;
                int productId = 0;

                using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                            productId = Convert.ToInt32(reader["ProductID"]);
                        }
                    }
                }

                if (stockQuantity > 0 && productId > 0) // Proceed only if valid data was retrieved
                {
                    // Step 2: Confirm inventory status
                    using (SqlCommand updateInventoryCmd = new SqlCommand(updateInventoryQuery, conn))
                    {
                        updateInventoryCmd.Parameters.AddWithValue("@InventoryID", inventoryID);
                        updateInventoryCmd.ExecuteNonQuery();
                    }

                    // Step 3: Add StockQuantity to the Quantity in Products table
                    using (SqlCommand updateProductCmd = new SqlCommand(updateProductQuantityQuery, conn))
                    {
                        updateProductCmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                        updateProductCmd.Parameters.AddWithValue("@ProductID", productId);
                        updateProductCmd.ExecuteNonQuery();
                    }
                }
            }

            // Reload the inventory data to reflect the changes
            loadInventoryData();
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

            decimal pricePerUnit = 0;
            int stockQuantity = 0;

            // Validation for quotation title and file upload
            if (productDL.SelectedValue == "0")
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Product must be selected!";
                isValid = false;
            }
            else if (string.IsNullOrEmpty(txtPrice.Text.Trim()) || !decimal.TryParse(txtPrice.Text.Trim(), out pricePerUnit))
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Price field can not be blancked!";
                isValid = false;
            }
            else if (string.IsNullOrEmpty(txtStock.Text.Trim()) || !int.TryParse(txtStock.Text.Trim(), out stockQuantity))
            {
                lblErrorMessage.Style["visibility"] = "visible";
                lblErrorMessage.Text = "Inventory stock field can not be blancked!";
                isValid = false;
            }

            if (isValid)
            {
                InventoryService.InventoryServiceSoapClient client = new InventoryService.InventoryServiceSoapClient();

                int productID = int.Parse(productDL.SelectedValue);
                int supplierID = (int)Session["UserId"];


                bool result = client.AddStock(productID, supplierID, pricePerUnit, stockQuantity);


                //Handle message for success or failure
                if (result)
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Style["color"] = "green";
                    lblErrorMessage.Text = "Stock updated successfully!";
                    Response.Redirect("InventoryManagment.aspx");
                }
                else
                {
                    lblErrorMessage.Style["visibility"] = "visible";
                    lblErrorMessage.Text = "Failed to update Stock!";
                }
            }
        }

        private void PopulateProducts()
        {
            string userID = Session["UserID"].ToString();
            // Assuming you have a Suppliers table with SupplierID and SupplierName columns
            string query = "SELECT ProductID, ProductName FROM Products WHERE SupplierID = @UserID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameter to avoid SQL injection
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        productDL.DataSource = reader;
                        productDL.DataTextField = "ProductName"; // Set the text to display
                        productDL.DataValueField = "ProductID"; // Set the value for each item
                        productDL.DataBind();
                    }
                }
            }
            productDL.Items.Insert(0, new ListItem("Select Product...", "0"));
        }
    }
}