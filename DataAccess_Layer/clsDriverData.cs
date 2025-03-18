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
using Contracts.Contracts.Order;
using Contracts.Contracts;

namespace DataAccess_Layer
{
    public class clsDriverData
    {

        public static int AddNewDriver(bool IsAvailable, int WeightCapacity, string Vehicle, int UserID, int SupplierID)
        {
            int ID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Driver ( 
                                IsAvailable, WeightCapacity, Vehicle, UserID, SupplierID)
                                VALUES (@IsAvailable, @WeightCapacity, @Vehicle, @UserID, @SupplierID);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsAvailable", IsAvailable);
                    command.Parameters.AddWithValue("@WeightCapacity", WeightCapacity);
                    command.Parameters.AddWithValue("@Vehicle", Vehicle);
                    command.Parameters.AddWithValue("@UserID", UserID);
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
                        // Handle exception
                    }
                }
            }
            return ID;
        }

        public static bool UpdateDriver(int DriverID, bool IsAvailable, int WeightCapacity, string Vehicle, int UserID, int SupplierID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Driver SET 
                                IsAvailable = @IsAvailable,
                                WeightCapacity = @WeightCapacity,
                                Vehicle = @Vehicle,
                                UserID = @UserID,
                                SupplierID = @SupplierID
                                WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@IsAvailable", IsAvailable);
                    command.Parameters.AddWithValue("@WeightCapacity", WeightCapacity);
                    command.Parameters.AddWithValue("@Vehicle", Vehicle);
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@SupplierID", SupplierID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool GetDriverInfoByDriverID(int DriverID, ref bool IsAvailable, ref int WeightCapacity, ref string Vehicle, ref int UserID, ref int SupplierID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Driver WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                IsAvailable = (bool)reader["IsAvailable"];
                                WeightCapacity = (int)reader["WeightCapacity"];
                                Vehicle = (string)reader["Vehicle"];
                                UserID = (int)reader["UserID"];
                                SupplierID = (int)reader["SupplierID"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static bool DeleteDriver(int DriverID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Driver WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsDriverExist(int DriverID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT 1 FROM Driver WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                    catch (Exception ex)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetAllDriver()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Driver";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                    }
                }
            }
            return dt;
        }

        public static async Task<List<DriverOrdersDTO>> GetDriverOrders(int DriverID)
        {
            var driverOrders = new List<DriverOrdersDTO>();
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("GetDriverOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                driverOrders.Add(new DriverOrdersDTO
                                {
                                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    OrderStatus = reader.GetByte(reader.GetOrdinal("OrderStatus"))
                                });


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return driverOrders;
        }

    }
}