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
using Contracts.Contracts;
using Contracts.Contracts.Order;

namespace DataAccess_Layer
{
    public class clsOrderData
    {

        public static int AddNewOrder(OrderRequestDTO orderRequestDTO)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO [Order] ( 
                            TotalAmount, OrderStatus, Quantity, OrderDate, ReceiveDate, Address, Feedback, CustomerID, ProductID, DriverID)
                            VALUES (@TotalAmount, @OrderStatus, @Quantity, @OrderDate, @ReceiveDate, @Address, @Feedback, @CustomerID, @ProductID, @DriverID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TotalAmount", orderRequestDTO.TotalAmount);
            command.Parameters.AddWithValue("@OrderStatus", orderRequestDTO.OrderStatus);
            command.Parameters.AddWithValue("@Quantity", orderRequestDTO.Quantity);
            command.Parameters.AddWithValue("@OrderDate", orderRequestDTO.OrderDate);
            command.Parameters.AddWithValue("@Address", orderRequestDTO.Address);
            command.Parameters.AddWithValue("@CustomerID", orderRequestDTO.CustomerID);
            command.Parameters.AddWithValue("@ProductID", orderRequestDTO.ProductID);
            command.Parameters.AddWithValue("@DriverID", orderRequestDTO.DriverID);
            command.Parameters.AddWithValue("@Feedback", orderRequestDTO.Feedback ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ReceiveDate", orderRequestDTO.ReceiveDate ?? (object)DBNull.Value);

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
        public static bool UpdateOrder(int OrderID, decimal TotalAmount, byte OrderStatus, int Quantity, DateTime OrderDate, DateTime? ReceiveDate, string Address, string Feedback, int CustomerID, int ProductID, int DriverID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Order set 
                            
TotalAmount = @TotalAmount,
OrderStatus = @OrderStatus,
Quantity = @Quantity,
OrderDate = @OrderDate,
ReceiveDate = @ReceiveDate,
Address = @Address,
Feedback = @Feedback,
CustomerID = @CustomerID,
ProductID = @ProductID,
DriverID = @DriverID
                            where OrderID = @OrderID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrderID", OrderID);
            command.Parameters.AddWithValue("@TotalAmount", TotalAmount);
            command.Parameters.AddWithValue("@OrderStatus", OrderStatus);
            command.Parameters.AddWithValue("@Quantity", Quantity);
            command.Parameters.AddWithValue("@OrderDate", OrderDate);
            command.Parameters.AddWithValue("@ReceiveDate", ReceiveDate);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Feedback", Feedback);
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            command.Parameters.AddWithValue("@ProductID", ProductID);
            command.Parameters.AddWithValue("@DriverID", DriverID);

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
        public static OrderRequestDTO GetOrderInfoByOrderID(int OrderID)
        {
            OrderRequestDTO orderRequestDTO = new OrderRequestDTO();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM [Order] WHERE OrderID = @OrderID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrderID", OrderID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found

                    orderRequestDTO.TotalAmount = (decimal)reader["TotalAmount"];
                    orderRequestDTO.OrderStatus = (byte)reader["OrderStatus"];
                    orderRequestDTO.Quantity = (int)reader["Quantity"];
                    orderRequestDTO.OrderDate = (DateTime)reader["OrderDate"];
                    orderRequestDTO.ReceiveDate = reader["ReceiveDate"] != DBNull.Value
                        ? (DateTime?)reader["ReceiveDate"]
                        : null;
                    orderRequestDTO.Address = (string)reader["Address"];
                    orderRequestDTO.Feedback = reader["Feedback"] != DBNull.Value
                        ? reader["Feedback"].ToString()
                        : null;
                    orderRequestDTO.CustomerID = (int)reader["CustomerID"];
                    orderRequestDTO.ProductID = (int)reader["ProductID"];
                    orderRequestDTO.DriverID = (int)reader["DriverID"];

                }
                else
                {
                    // The record was not found
                    orderRequestDTO = null;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                orderRequestDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return orderRequestDTO;
        }
        public static bool DeleteOrder(int OrderID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Order 
                            where OrderID = @OrderID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrderID", OrderID);

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
        public static bool IsOrderExist(int OrderID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Order WHERE OrderID = @OrderID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrderID", OrderID);

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
        public static DataTable GetAllOrder()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Order";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
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

        public static async Task<RevenueDto> GetTotalRevenuesAsync()
        {
            using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (var command = new SqlCommand("GetTotalRevenues", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Declare output parameters
                    command.Parameters.Add(new SqlParameter("@CurrentTotalRevenue", SqlDbType.SmallMoney) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@RevenuePercentage", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@CurrentSales", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@SalesPercentage", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@CurrentTodaySales", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@TodaySalesPercentage", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    return new RevenueDto
                    {
                        CurrentTotalRevenue = (command.Parameters["@CurrentTotalRevenue"].Value != DBNull.Value) ? Convert.ToDecimal(command.Parameters["@CurrentTotalRevenue"].Value) : 0,
                        RevenuePercentage = (command.Parameters["@RevenuePercentage"].Value != DBNull.Value) ? Convert.ToInt32(command.Parameters["@RevenuePercentage"].Value) : 0,
                        CurrentSales = (command.Parameters["@CurrentSales"].Value != DBNull.Value) ? Convert.ToInt32(command.Parameters["@CurrentSales"].Value) : 0,
                        SalesPercentage = (command.Parameters["@SalesPercentage"].Value != DBNull.Value) ? Convert.ToInt32(command.Parameters["@SalesPercentage"].Value) : 0,
                        CurrentTodaySales = (command.Parameters["@CurrentTodaySales"].Value != DBNull.Value) ? Convert.ToInt32(command.Parameters["@CurrentTodaySales"].Value) : 0,
                        TodaySalesPercentage = (command.Parameters["@TodaySalesPercentage"].Value != DBNull.Value) ? Convert.ToInt32(command.Parameters["@TodaySalesPercentage"].Value) : 0
                    };
                }
            }
        }

        public static async Task<List<RecentSalesDTO>> GetRecentSalesAsync()
        {
            var salesList = new List<RecentSalesDTO>();

            using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (var command = new SqlCommand("RecentSales", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            salesList.Add(new RecentSalesDTO
                            {
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                            });
                        }
                    }
                }
            }

            return salesList;
        }

        public static async Task<List<OrdersPerMonthDTO>> GetProductsForAllMonthsAsync()
        {
            var salesList = new List<OrdersPerMonthDTO>();

            using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (var command = new SqlCommand("GetProductsForAllMonths", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            salesList.Add(new OrdersPerMonthDTO
                            {
                                Month = Convert.ToInt32(reader["Month"]),
                                Orders = Convert.ToInt32(reader["Orders"])
                            });
                        }
                    }
                }
            }

            return salesList;
        }

        public static async Task<List<CustomerOrdersDTO>> GetCustomerOrders(int CustomerID)
        {
            var customerOrders = new List<CustomerOrdersDTO>();
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("GetCustomerOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@cstmID", CustomerID);
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customerOrders.Add(new CustomerOrdersDTO
                                {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    ProductName = reader["ProdcutName"].ToString(),
                                    Weight = Convert.ToInt32(reader["Weight"]),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    ReceiveDate = reader["ReceiveDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReceiveDate"]) : null,
                                    OrderStatus = Convert.ToByte(reader["OrderStatus"]),  // Convert to byte instead of int
                                    Image = reader["Image"] != DBNull.Value ? reader["Image"].ToString() : null
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {

            }


            return customerOrders;
        }

    }
}
