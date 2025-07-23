using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFix_Client_Application.ProductService;

namespace TechFix_Client_Application
{
    public partial class StaffDashboard : System.Web.UI.Page
    {
        string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    overlay.Style["visibility"] = "hidden";
                    popup.Style["visibility"] = "hidden";
                    mainmessagebox.Style["visibility"] = "hidden";

                    string username = Session["Username"] != null ? Session["Username"].ToString() : "User";
                    profileBtn.InnerText = $"Hi! {username}";
                    PopulateSupplierDropDownList();

                    getAllListedProducts();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            overlay.Style["visibility"] = "hidden";
            popup.Style["visibility"] = "hidden";
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton deleteButton = (LinkButton)sender;
            int cartItemId = int.Parse(deleteButton.CommandArgument);
            string query = "DELETE FROM CartItems WHERE CartItemID = @CartItemID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CartItemID", cartItemId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh the cart details after deletion
            GetCartDetails();
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            Button btnProceed = (Button)sender;
            int cartItemId = Convert.ToInt32(btnProceed.CommandArgument);
            int userId = (int)Session["UserId"];

            // Retrieve cart item details
            string getCartItemQuery = @"
                SELECT ci.ProductID, ci.Quantity, ci.Price AS TotalAmount, ci.UnitPrice
                FROM CartItems ci
                WHERE ci.CartItemID = @CartItemID";

            int productId = 0;
            int quantity = 0;
            decimal totalAmount = 0;
            decimal unitPrice = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(getCartItemQuery, conn);
                cmd.Parameters.AddWithValue("@CartItemID", cartItemId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productId = Convert.ToInt32(reader["ProductID"]);
                        quantity = Convert.ToInt32(reader["Quantity"]);
                        totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        unitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    }
                }
            }

            // Insert Order details
            string insertOrderQuery = @"
                INSERT INTO [Orders] (UserID, OrderDate, TotalAmount, Status, ProductID, Quantity) 
                VALUES (@UserID, @OrderDate, @TotalAmount, @Status, @ProductID, @Quantity)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertOrderQuery, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@Status", "Pending");
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Delete the item from CartItems table after placing the order
            string deleteCartItemQuery = "DELETE FROM CartItems WHERE CartItemID = @CartItemID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(deleteCartItemQuery, conn);
                cmd.Parameters.AddWithValue("@CartItemID", cartItemId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            GetCartDetails();

        }


        private void getAllListedProducts()
        {
            ProductService.ProductServiceSoapClient client = new ProductService.ProductServiceSoapClient();
            List<ProductCard> productCards = client.GetAllListedProducts().ToList();
            ProductRepeater.DataSource = productCards;
            ProductRepeater.DataBind();
        }


        private void PopulateSupplierDropDownList()
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierID, SupplierName FROM Suppliers";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    supplierDL.DataSource = reader;
                    supplierDL.DataTextField = "SupplierName"; // This will be the text displayed in dropdown
                    supplierDL.DataValueField = "SupplierID";   // This will be the value behind each dropdown option
                    supplierDL.DataBind();

                    // Optional: Add a default item
                    supplierDL.Items.Insert(0, new ListItem("Select a Supplier", "0"));
                }
            }
        }

        protected void btnAddtoCart_Click(object sender, EventArgs e)
        {
            // Get the button that triggered the event and retrieve the product ID and entered quantity
            Button addToCartButton = (Button)sender;

            string[] arguments = addToCartButton.CommandArgument.Split('|');

            int productId = int.Parse(arguments[0]);
            decimal price = Decimal.Parse(arguments[1]);

            // Find the parent container to access the quantity textbox
            RepeaterItem item = (RepeaterItem)addToCartButton.NamingContainer;
            TextBox txtItemCount = (TextBox)item.FindControl("txtItemCount");

            if(string.IsNullOrEmpty(txtItemCount.Text))
            {
                mainmessagebox.Style["visibility"] = "visible";
                mainMessage.Text = "Product Quantity field is empty!";
                overlay.Style["visibility"] = "visible";
            }

            int requestedQuantity;
            if (!int.TryParse(txtItemCount.Text, out requestedQuantity) || requestedQuantity <= 0)
            {
                // Display an error if the input is invalid
                return;
            }

            // Retrieve available quantity from database
            int availableQuantity = 0;
            string query = "SELECT Quantity FROM Products WHERE ProductID = @ProductID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        availableQuantity = Convert.ToInt32(result);
                    }
                }
            }

            // Check if requested quantity is available
            if (requestedQuantity > availableQuantity)
            {
                // Display an error message if requested quantity exceeds available stock
                //lblError.Text = "The requested quantity exceeds available stock.";
                //lblError.Visible = true;
                mainmessagebox.Style["visibility"] = "visible";
                mainMessage.Text = "The requested quantity exceeds available stock.";
                overlay.Style["visibility"] = "visible";
                return;
            }
            else
            {
                // Add item to the cart
                AddItemToCart(productId, requestedQuantity, price);
                mainmessagebox.Style["visibility"] = "visible";
                mainMessage.Text = "Product Added to the Cart Successfully!";
                overlay.Style["visibility"] = "visible";
            }
        }

        private void AddItemToCart(int productId, int quantity, decimal unitPrice)
        {
            int userId = (int)Session["UserId"];
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Cart WHERE UserID = @UserID AND IsActive = 1)
                BEGIN
                    INSERT INTO Cart (UserID, CreatedDate, IsActive) VALUES (@UserID, GETDATE(), 1);
                END;
        
                DECLARE @CartID INT = (SELECT CartID FROM Cart WHERE UserID = @UserID AND IsActive = 1);
        
                IF EXISTS (SELECT 1 FROM CartItems WHERE CartID = @CartID AND ProductID = @ProductID)
                BEGIN
                    UPDATE CartItems 
                    SET Quantity = Quantity + @Quantity, 
                        Price = Price + (@UnitPrice * @Quantity)
                    WHERE CartID = @CartID AND ProductID = @ProductID;
                END
                ELSE
                BEGIN
                    INSERT INTO CartItems (CartID, ProductID, Quantity, Price, UnitPrice)
                    VALUES (@CartID, @ProductID, @Quantity, @unitPrice * @Quantity, @unitPrice);
                END;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void GetCartDetails()
        {
            int userId = (int)Session["UserId"];
            string checkCartQuery = @"
                SELECT CartID 
                FROM Cart 
                WHERE UserID = @UserID AND IsActive = 1";

            int cartId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(checkCartQuery, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    cartId = Convert.ToInt32(result);
                }
                else
                {
                    CartRepeater.Visible = false;
                    lblCartMessage.Visible = true;
                    lblCartMessage.Text = "CART IS EMPTY!";
                    return;
                }
            }

            string query = @"
                SELECT ci.CartItemID, ci.CartID, ci.ProductID, ci.Quantity, ci.Price, 
                       p.ProductName, ci.UnitPrice, s.SupplierName
                FROM CartItems ci
                INNER JOIN Products p ON ci.ProductID = p.ProductID
                INNER JOIN Suppliers s ON p.SupplierID = s.SupplierID
                WHERE ci.CartID = @CartID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@CartID", cartId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    CartRepeater.DataSource = dt;
                    CartRepeater.DataBind();
                    CartRepeater.Visible = true;
                    lblCartMessage.Visible = false;
                }
                else
                {
                    CartRepeater.Visible = false;
                    lblCartMessage.Visible = true;
                    lblCartMessage.Text = "CART IS EMPTY!";
                }
            }
        }
        protected void btnMessageClose_Click(object sender, EventArgs e)
        {
            overlay.Style["visibility"] = "hidden";
           mainmessagebox.Style["visibility"] = "hidden";
        }


        protected void btnViewCart_Click(object sender, EventArgs e)
        {
            overlay.Style["visibility"] = "visible";
            popup.Style["visibility"] = "visible";

            GetCartDetails();
        }


        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = searchbox.Text.Trim();
            int supplierID = int.Parse(supplierDL.SelectedValue);

            // Clear the search message
            lblSearchMessage.Visible = false;

            // Check if both search term and supplierID are empty/default
            if (string.IsNullOrEmpty(searchTerm) && supplierID == 0)
            {
                ProductRepeater.Visible = true; // Ensure ProductRepeater is visible
                getAllListedProducts();
                return; // Exit the method to avoid further processing
            }

            ProductService.ProductServiceSoapClient client = new ProductService.ProductServiceSoapClient();
            List<ProductCard> productCards = client.SearchProducts(searchTerm, supplierID).ToList();

            if (productCards.Count > 0)
            {
                ProductRepeater.Visible = true; // Ensure ProductRepeater is visible
                ProductRepeater.DataSource = productCards;
                ProductRepeater.DataBind();
            }
            else
            {
                ProductRepeater.Visible = false;
                lblSearchMessage.Visible = true;
                lblSearchMessage.Text = "No Item found!";
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