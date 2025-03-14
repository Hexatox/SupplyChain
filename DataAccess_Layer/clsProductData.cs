using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;
using System.ComponentModel;
using Backend.Contracts;

namespace DataAccess_Layer
{
    public class clsProductData
    {

        public static int AddNewProduct(string ProdcutName, int Quantity, decimal Price, int Weight, int SupplierID)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Product ( 
                            ProdcutName, Quantity, Price, Weight, SupplierID)
                            VALUES (@ProdcutName, @Quantity, @Price, @Weight, @SupplierID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProdcutName", ProdcutName);
            command.Parameters.AddWithValue("@Quantity", Quantity);
            command.Parameters.AddWithValue("@Price", Price);
            command.Parameters.AddWithValue("@Weight", Weight);
            command.Parameters.AddWithValue("@SupplierID", SupplierID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return ID;
        }
        public static bool UpdateProduct(int ProductID, string ProdcutName, int Quantity, decimal Price, int Weight, int SupplierID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Product set 
                            
ProdcutName = @ProdcutName,
Quantity = @Quantity,
Price = @Price,
Weight = @Weight,
SupplierID = @SupplierID
                            where ProductID = @ProductID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductID", ProductID);
            command.Parameters.AddWithValue("@ProdcutName", ProdcutName);
            command.Parameters.AddWithValue("@Quantity", Quantity);
            command.Parameters.AddWithValue("@Price", Price);
            command.Parameters.AddWithValue("@Weight", Weight);
            command.Parameters.AddWithValue("@SupplierID", SupplierID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static bool GetProductInfoByProductID(int ProductID, ref string ProdcutName, ref int Quantity, ref decimal Price, ref int Weight, ref int SupplierID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Product WHERE ProductID = @ProductID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductID", ProductID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    ProdcutName = (string)reader["ProdcutName"];
                    Quantity = (int)reader["Quantity"];
                    Price = (decimal)reader["Price"];
                    Weight = (int)reader["Weight"];
                    SupplierID = (int)reader["SupplierID"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool DeleteProduct(int ProductID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Product 
                            where ProductID = @ProductID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductID", ProductID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }
        public static bool IsProductExist(int ProductID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Product WHERE ProductID = @ProductID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProductID", ProductID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static  List<ProductResponseDTO> GetAllProduct()
        {

            List<ProductResponseDTO> dt = new List<ProductResponseDTO>();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Product";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var obj = new ProductResponseDTO
                    {
                        ProdcutName = reader["ProdcutName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToInt32(reader["Price"]),
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        
                    };
                    
                    dt.Add(obj);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

    }
}
