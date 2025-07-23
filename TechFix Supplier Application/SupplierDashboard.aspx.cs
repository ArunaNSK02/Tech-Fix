using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechFix_Supplier_Application.ProductService;

namespace TechFix_Supplier_Application
{
    public partial class SupplierDashboard : System.Web.UI.Page
    {
        private string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {

                    overlay.Style["visibility"] = "hidden";

                    string name = Session["Name"] != null ? Session["Name"].ToString() : "User";
                    profileBtn.InnerText = $"Hi! {name}";
                    GetAllProductsOfSupplier();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = searchbox.Text.Trim();
            int userId = (int)Session["UserId"];

            // Clear the search message
            lblSearchMessage.Visible = false;

            // Check if both search term and supplierID are empty/default
            if (string.IsNullOrEmpty(searchTerm))
            {
                ProductRepeater.Visible = true; // Ensure ProductRepeater is visible
                GetAllProductsOfSupplier();
                return; // Exit the method to avoid further processing
            }



            ProductService.ProductServiceSoapClient client = new ProductService.ProductServiceSoapClient();
            List<ProductCard> productCards = client.SearchProducts(searchTerm, userId).ToList();

            if (productCards.Count > 0)
            {
                ProductRepeater.Visible = true; // Ensure ProductRepeater is visible
                ProductRepeater.DataSource = productCards;
                ProductRepeater.DataBind();
                Session["searchTerm"] = searchTerm;
            }
            else
            {
                ProductRepeater.Visible = false;
                lblSearchMessage.Visible = true;
                lblSearchMessage.Text = "No Item found!";
            }
        }


        





        private void GetAllProductsOfSupplier()
        {
            int userId = (int)Session["UserId"];
            string searchTerm = null;

            ProductService.ProductServiceSoapClient client = new ProductService.ProductServiceSoapClient();
            List<ProductCard> productCards = client.SearchProducts(searchTerm, userId).ToList();

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

 

        
    }
}