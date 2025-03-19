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

namespace DataAccess_Layer
{
    public class clsNotificationData
    {

        public static int AddNewNotification(string Message, DateTime Date, int UserID)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Notification ( 
                            Message, Date, UserID)
                            VALUES (@Message, @Date, @UserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Message", Message);
            command.Parameters.AddWithValue("@Date", Date);
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
        public static bool UpdateNotification(int NotificationID, string Message, DateTime Date, int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Notification set 
                            
Message = @Message,
Date = @Date,
UserID = @UserID
                            where NotificationID = @NotificationID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NotificationID", NotificationID);
            command.Parameters.AddWithValue("@Message", Message);
            command.Parameters.AddWithValue("@Date", Date);
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
        public static bool GetNotificationInfoByNotificationID(int NotificationID, ref string Message, ref DateTime Date, ref int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Notification WHERE NotificationID = @NotificationID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NotificationID", NotificationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    Message = (string)reader["Message"];
                    Date = (DateTime)reader["Date"];
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
        public static bool DeleteNotification(int NotificationID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Notification 
                            where NotificationID = @NotificationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NotificationID", NotificationID);

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
        public static bool IsNotificationExist(int NotificationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Notification WHERE NotificationID = @NotificationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NotificationID", NotificationID);

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
        public static DataTable GetAllNotification()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Notification";

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

        public static bool SendMessage(NotificationRequestDTO notificationRequestDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SendMessage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SenderUserID", notificationRequestDTO.SenderUserID);
                        command.Parameters.AddWithValue("@ReceiverEmail", notificationRequestDTO.ReceiverEmail);
                        command.Parameters.AddWithValue("@Subject", notificationRequestDTO.Subject);
                        command.Parameters.AddWithValue("@Message", notificationRequestDTO.Message);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { 
                rowsAffected = 0;
            }
            
            return rowsAffected > 0;
        }

    }
}