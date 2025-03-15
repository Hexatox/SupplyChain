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
using Contracts.Contracts;

namespace DataAccess_Layer
{
    public class clsProductData
    {
        // Convert image file to byte array
        //public byte[] ConvertImageToBytes(string imagePath)
        //{
        //    return File.ReadAllBytes(imagePath);
        //}

        //// Example usage:
        //byte[] imageBytes = ConvertImageToBytes("C:\\Images\\product.jpg");
        //command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = imageBytes;

        public static int AddNewProduct(ProductRequestDTO productRequestDTO)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Product ( 
                            ProdcutName, Quantity, Price, Weight, SupplierID, Cost, Description)
                            VALUES (@ProdcutName, @Quantity, @Price, @Weight, @SupplierID
, @Cost, @Description);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProdcutName", productRequestDTO.ProdcutName);
            command.Parameters.AddWithValue("@Quantity", productRequestDTO.Quantity);
            command.Parameters.AddWithValue("@Price", productRequestDTO.Price);
            command.Parameters.AddWithValue("@Weight", productRequestDTO.Weight);
            command.Parameters.AddWithValue("@SupplierID", productRequestDTO.SupplierID);
            command.Parameters.AddWithValue("@Cost", productRequestDTO.Cost);
            //command.Parameters.AddWithValue("@Image", productRequestDTO.Image);
            command.Parameters.AddWithValue("@Description", productRequestDTO.Description);

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
        public static ProductRequestDTO GetProductInfoByProductID(int ProductID)
        {
            ProductRequestDTO productRequestDTO = new ProductRequestDTO();

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
                    productRequestDTO.ProductID = (int)reader["ProductID"];
                    productRequestDTO.ProdcutName = (string)reader["ProdcutName"];
                    productRequestDTO.Quantity = (int)reader["Quantity"];
                    productRequestDTO.Price = (decimal)reader["Price"];
                    productRequestDTO.Weight = (int)reader["Weight"];
                    productRequestDTO.SupplierID = (int)reader["SupplierID"];
                    productRequestDTO.Cost = (decimal)reader["Cost"];
                    productRequestDTO.Description = reader["Description"].ToString();
                    productRequestDTO.Image = reader["Image"] != DBNull.Value
                        ? Convert.ToBase64String((byte[])reader["Image"])
                        : null;
                    reader.Close();

                    return productRequestDTO;
                }
                else
                {
                    // The record was not found
                    reader.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
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


        public async static  Task<List<ProductResponseDTO>> GetAllProduct()
        {
            var Products = new List<ProductResponseDTO>();

            using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (var command = new SqlCommand("GetAllProduct", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Products.Add(new ProductResponseDTO
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProdcutName = reader["ProdcutName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }
            }

            return Products;
        }


    }
}
