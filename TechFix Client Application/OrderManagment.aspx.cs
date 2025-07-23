using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TechFix_Client_Application
{
    public partial class OrderManagment : System.Web.UI.Page
    {
        string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {

                    string username = Session["Username"] != null ? Session["Username"].ToString() : "User";
                    profileBtn.InnerText = $"Hi! {username}";

                    LoadOrders();

                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
            LinkButton deleteButton = (LinkButton)sender;
            int orderId = int.Parse(deleteButton.CommandArgument);
            string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the parameter to the SQL command
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadOrders();
        }

        private void LoadOrders()
        {
            int userId = (int)Session["UserId"];

            string query = @"
                SELECT 
                    o.OrderID,
                    o.OrderDate,
                    o.Quantity,
                    o.TotalAmount,
                    o.Status,
                    p.ProductName,
                    s.SupplierName
                FROM 
                    Orders o
                JOIN 
                    Products p ON o.ProductID = p.ProductID
                JOIN 
                    Suppliers s ON p.SupplierID = s.SupplierID
                WHERE 
                    o.UserID = @UserID;";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@UserID", userId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    orderRepeater.DataSource = dt;
                    orderRepeater.DataBind();
                    orderRepeater.Visible = true;
                    lblOrderMessage.Visible = false;
                }
                else
                {
                    orderRepeater.Visible = false;
                    lblOrderMessage.Visible = true;
                    lblOrderMessage.Text = "No Orders Found!";
                }
            }

        }

        protected void orderRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Get the status of the order
                string status = DataBinder.Eval(e.Item.DataItem, "Status").ToString();

                // Find the controls in the item (delete button and delivered label)
                var deleteLink = (LinkButton)e.Item.FindControl("deleteLink");
                var deliveredLabel = (Label)e.Item.FindControl("deliveredLabel");

                // Show or hide the delete button and delivered label based on status
                if (status == "Pending")
                {
                    deleteLink.Visible = true; // Show the delete button for pending orders
                    deliveredLabel.Visible = false; // Hide the "Delivered" label
                }
                else if (status == "Delivered")
                {
                    deleteLink.Visible = false; // Hide the delete button for delivered orders
                    deliveredLabel.Visible = true; // Show the "Delivered" label
                }
            }
        }




    }
}