using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web_Services
{
    /// <summary>
    /// Summary description for InventoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InventoryService : System.Web.Services.WebService
    {

        private readonly string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";

        [WebMethod]
        public bool AddStock(int productId, int supplierId, decimal price, int stockQuantity)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Inventory (ProductID, SupplierID, Price, StockQuantity, LastUpdated, Status) " +
                                   "VALUES (@ProductID, @SupplierID, @Price, @StockQuantity, @StockInsertDate, 'Pending')";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    command.Parameters.AddWithValue("@SupplierID", supplierId);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@StockInsertDate", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [WebMethod]
        public List<Inventory> GetAllInventories(int supplierID)
        {
            List<Inventory> inventories = new List<Inventory>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT i.InventoryID, i.ProductID, i.SupplierID, i.Price, i.StockQuantity, " +
                               "i.LastUpdated, i.Status, p.ProductName, s.SupplierName " +
                               "FROM Inventory i " +
                               "JOIN Products p ON i.ProductID = p.ProductID " +
                               "JOIN Suppliers s ON i.SupplierID = s.SupplierID " +
                               "WHERE i.SupplierID = @SupplierID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add the SupplierID parameter to filter results
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inventories.Add(new Inventory
                            {
                                InventoryID = (int)reader["InventoryID"],
                                ProductID = (int)reader["ProductID"],
                                SupplierID = (int)reader["SupplierID"],
                                Price = (decimal)reader["Price"],
                                StockQuantity = (int)reader["StockQuantity"],
                                StockInsertDate = (DateTime)reader["LastUpdated"],
                                Status = (string)reader["Status"],
                                ProductName = (string)reader["ProductName"],
                                SupplierName = (string)reader["SupplierName"]
                            });
                        }
                    }
                }
            }

            return inventories;
        }

    }

    public class Inventory
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public decimal Price { get; set; }
        public DateTime StockInsertDate { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
    }
}
