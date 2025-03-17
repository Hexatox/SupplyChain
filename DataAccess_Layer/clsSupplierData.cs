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
using Contracts.Contracts.Order;
using Contracts.Contracts;

namespace DataAccess_Layer
{
    public class clsSupplierData
    {

        public static int AddNewSupplier(int UserID)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Supplier ( 
                            UserID)
                            VALUES (@UserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

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
        public static bool UpdateSupplier(int SupplierID, int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Supplier set 
                             
UserID = @UserID
                            where SupplierID = @SupplierID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SupplierID", SupplierID);
            command.Parameters.AddWithValue("@UserID", UserID);

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
        public static bool GetSupplierInfoBySupplierID(int SupplierID, ref int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Supplier WHERE SupplierID = @SupplierID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SupplierID", SupplierID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
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
        public static bool DeleteSupplier(int SupplierID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Supplier 
                            where SupplierID = @SupplierID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SupplierID", SupplierID);

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
        public static bool IsSupplierExist(int SupplierID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Supplier WHERE SupplierID = @SupplierID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SupplierID", SupplierID);

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
        public static List<SupplierResponseDTO> GetAllSupplier()
        {
            List<SupplierResponseDTO> dt = new List<SupplierResponseDTO>(); 
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Supplier";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var obj = new SupplierResponseDTO
                    {
                        UserName = reader["UserName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(), 
                        Password = reader["Password"].ToString(),
                        Address = reader["Address"].ToString(),
                        Name = reader["Name"].ToString(),
                        Permissions = Convert.ToInt32(reader["Permissions"].ToString())
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


        public static async Task<List<SupplierOrdersDTO>> GetSupplierOrders(int SupplierID)
        {
            var supplierOrders = new List<SupplierOrdersDTO>();
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("GetSupplierOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", SupplierID);
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                supplierOrders.Add(new SupplierOrdersDTO
                                {
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),  
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    OrderStatus = reader.GetByte(reader.GetOrdinal("OrderStatus")),
                                    DriverName = reader.GetString(reader.GetOrdinal("DriverName"))
                                });


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return supplierOrders;
        }

    }
}