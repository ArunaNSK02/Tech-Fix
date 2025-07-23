using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Web_Services
{
    /// <summary>
    /// Summary description for QuotationService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QuotationService : System.Web.Services.WebService
    {
        private readonly string connectionString = "Data Source=DESKTOP-DICH41S\\SQLEXPRESS;Initial Catalog=TechFixSOC;Integrated Security=True";

        [WebMethod]
        public int InsertQuotation(string title, string description, string filePath, int supplierId)
        {
            string query = "INSERT INTO Quotations (SupplierID, RequestDate, Status, Title, Attachment, Note, SupplierResponse) " +
                           "VALUES (@SupplierID, @RequestDate, @Status, @Title, @Attachment, @Note, @SupplierResponse)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                cmd.Parameters.AddWithValue("@RequestDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Status", "Pending");  // Assuming the status is 'Pending' by default
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Attachment", filePath);
                cmd.Parameters.AddWithValue("@Note", description);
                cmd.Parameters.AddWithValue("@SupplierResponse", DBNull.Value); // Supplier response is empty initially
                return cmd.ExecuteNonQuery();
            }
        }


        [WebMethod]
        public List<Quotation> GetAllQuotations()
        {
            List<Quotation> quotations = new List<Quotation>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT q.QuotationID, q.SupplierID, q.RequestDate, q.Status, q.Title, " +
                               "q.Attachment, q.Note, q.SupplierResponse, s.SupplierName " +
                               "FROM Quotations q " +
                               "JOIN Suppliers s ON q.SupplierID = s.SupplierID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            quotations.Add(new Quotation
                            {
                                QuotationID = (int)reader["QuotationID"],
                                SupplierID = (int)reader["SupplierID"],
                                RequestDate = (DateTime)reader["RequestDate"],
                                Status = (string)reader["Status"],
                                Title = (string)reader["Title"],
                                Attachment = reader["Attachment"] as string,  // Use as string to allow null values
                                Note = reader["Note"] as string,
                                SupplierResponse = reader["SupplierResponse"] as string,
                                SupplierName = (string)reader["SupplierName"]
                            });
                        }
                    }
                }
            }

            return quotations;
        }


        [WebMethod]
        public List<Quotation> GetAllQuotationsBySupplier(int supplierID)
        {
            List<Quotation> quotations = new List<Quotation>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT q.QuotationID, q.SupplierID, q.RequestDate, q.Status, q.Title, " +
                               "q.Attachment, q.Note, q.SupplierResponse, s.SupplierName " +
                               "FROM Quotations q " +
                               "JOIN Suppliers s ON q.SupplierID = s.SupplierID " +
                               "WHERE q.SupplierID = @SupplierID"; 

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            quotations.Add(new Quotation
                            {
                                QuotationID = (int)reader["QuotationID"],
                                SupplierID = (int)reader["SupplierID"],
                                RequestDate = ((DateTime)reader["RequestDate"]).Date,
                                Status = (string)reader["Status"],
                                Title = (string)reader["Title"],
                                Attachment = reader["Attachment"] as string,
                                Note = reader["Note"] as string,
                                SupplierResponse = reader["SupplierResponse"] as string,
                                SupplierName = (string)reader["SupplierName"]
                            });
                        }
                    }
                }
            }

            return quotations;
        }


    }

    public class Quotation
    {
        public int QuotationID { get; set; }
        public int SupplierID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Attachment { get; set; }
        public string Note { get; set; }
        public string SupplierResponse { get; set; }
        public string SupplierName { get; set; }
    }
}
