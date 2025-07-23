using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web_Services
{
    /// <summary>
    /// Summary description for ProductService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProductService : System.Web.Services.WebService
    {
        readonly string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";

        [WebMethod]
        public List<ProductCard> SearchProducts(string searchTerm, int supplierID)
        {
            List<ProductCard> productCards = new List<ProductCard>();

            // Start building the query
            string query = @"
            SELECT 
                p.ProductID,
                p.ProductName,
                p.Description,
                p.SupplierID,
                p.ImagePath,
                p.Quantity AS AvailableAmount,
                s.SupplierName,
                i.Price,
                i.LastUpdated
            FROM Products p
            JOIN Suppliers s ON p.SupplierID = s.SupplierID
            LEFT JOIN Inventory i ON p.ProductID = i.ProductID 
                AND i.Status = 'confirmed'
                AND i.LastUpdated = (
                    SELECT MAX(LastUpdated)
                    FROM Inventory
                    WHERE ProductID = p.ProductID
                      AND Status = 'confirmed'
                )
            WHERE 1=1
        ";

            // Add condition for search term if present
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " AND p.ProductName LIKE @SearchTerm";
            }

            // Add condition for selected supplier if specified
            if (supplierID != 0)
            {
                query += " AND p.SupplierID = @SupplierID";
            }

            query += " ORDER BY p.ProductName"; // Ordering by product name

            // Execute the query and populate the product cards list
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to avoid SQL injection
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%"); // Wildcards for partial matching
                    }

                    if (supplierID != 0)
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ProductCard productCard = new ProductCard
                        {
                            ProductID = (int)reader["ProductID"],
                            ProductName = reader["ProductName"].ToString(),
                            Description = reader["Description"].ToString(),
                            SupplierID = (int)reader["SupplierID"],
                            ImagePath = reader["ImagePath"].ToString(),
                            AvailableAmount = (int)reader["AvailableAmount"],
                            SupplierName = reader["SupplierName"].ToString(),
                            Price = reader["Price"] != DBNull.Value ? (decimal)reader["Price"] : 0,
                            LastUpdated = reader["LastUpdated"] != DBNull.Value ? (DateTime?)reader["LastUpdated"] : null
                        };

                        productCards.Add(productCard);
                    }
                }
            }

            return productCards;
        }





        [WebMethod]
        public int InsertProduct(string title, string description, string filePath, int supplierId)
        {
            string query = "INSERT INTO Products (SupplierID, ProductName, Description, ImagePath)" +
                           "VALUES (@SupplierID, @ProductName, @Description, @ImagePath)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                cmd.Parameters.AddWithValue("@ProductName", title);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@ImagePath", filePath);
                
                return cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public List<ProductCard> GetAllListedProducts()
        {
            List<ProductCard> productCards = new List<ProductCard>();

            string query = @"
                SELECT 
                    p.ProductID,
                    p.ProductName,
                    p.Description,
                    p.SupplierID,
                    p.ImagePath,
                    p.Quantity AS AvailableAmount,
                    s.SupplierName,
                    i.Price,
                    i.LastUpdated
                FROM Products p
                JOIN Suppliers s ON p.SupplierID = s.SupplierID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID 
                    AND i.Status = 'confirmed'
                    AND i.LastUpdated = (
                        SELECT MAX(LastUpdated)
                        FROM Inventory
                        WHERE ProductID = p.ProductID
                          AND Status = 'confirmed'
                    )
                ORDER BY p.ProductName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductCard card = new ProductCard
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                            ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath")),
                            AvailableAmount = reader.GetInt32(reader.GetOrdinal("AvailableAmount")),
                            SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                            Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price")),
                            LastUpdated = reader.IsDBNull(reader.GetOrdinal("LastUpdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastUpdated"))
                        };

                        productCards.Add(card);
                    }
                }
            }

            return productCards;
        }
    }


    

    public class ProductCard
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int SupplierID { get; set; }
        public string ImagePath { get; set; }
        public int AvailableAmount { get; set; }
        public string SupplierName { get; set; }
        public decimal Price { get; set; }
        public DateTime? LastUpdated { get; set; }
    }


    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public string SupplierName { get; set; }
    }
}
