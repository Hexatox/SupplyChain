using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Backend.Contracts; // Updated namespace

namespace DataAccess_Layer
{
    public class clsCustomerData
    {
        public static int AddNewCustomer(CustomerRequestDTO Customer)
        {
            int ID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Customer (UserID)
                                 VALUES (@UserID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", Customer.UserID);

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
                        // Log error
                    }
                }
            }
            return ID;
        }

        public static bool UpdateCustomer(CustomerRequestDTO Customer)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Customer SET UserID = @UserID WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", Customer.CustomerID);
                    command.Parameters.AddWithValue("@UserID", Customer.UserID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static CustomerResponseDTO GetCustomerInfoByCustomerID(int CustomerID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT UserID FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CustomerResponseDTO user = new CustomerResponseDTO(
                                    CustomerID, (int)reader["UserID"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        return null;
                    }
                }
            }
            return null;
        }

        public static bool DeleteCustomer(int CustomerID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log error
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsCustomerExist(int CustomerID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT 1 FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);

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
                        // Log error
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static List<CustomerResponseDTO> GetAllCustomer()
        {
            List<CustomerResponseDTO> dt = new List<CustomerResponseDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Customer";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dt.Add(new CustomerResponseDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("UserID")),
                                    reader.GetInt32(reader.GetOrdinal("CustomerID"))
                                ));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error
                    }
                }
            }
            return dt;
        }

    }
}
