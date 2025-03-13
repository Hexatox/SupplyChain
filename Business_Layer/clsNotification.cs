using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsNotification
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int NotificationID {set;get;}
        public string Message {set;get;}
        public DateTime Date {set;get;}
        public int UserID {set;get;}
        public clsUser User {set;get;}
    public clsNotification (){
        this.NotificationID = -1;
        this.Message = "";
        this.Date = DateTime.Now;
        this.UserID = -1;


        this.Mode = enMode.AddNew;
}
    private clsNotification (int NotificationID, string Message, DateTime Date, int UserID){
        this.NotificationID = NotificationID;
        this.Message = Message;
        this.Date = Date;
        this.UserID = UserID;

        this.User = clsUser.Find(UserID);


        this.Mode = enMode.Update;
}
    private bool _AddNewNotification(){
        this.NotificationID = clsNotificationData.AddNewNotification(this.Message, this.Date, this.UserID);
        return (this.NotificationID != -1);
    }
    private bool _UpdateNotification(){
        return clsNotificationData.UpdateNotification(this.NotificationID, this.Message, this.Date, this.UserID);
    }
    public static clsNotification Find(int NotificationID){
        string Message = "";
        DateTime Date = DateTime.Now;
        int UserID = -1;

        bool IsFound = clsNotificationData.GetNotificationInfoByNotificationID(
            NotificationID, ref Message, ref Date, ref UserID);

        if (IsFound){
            return new clsNotification(NotificationID, Message, Date, UserID);}
        else{ return null;}
    }
    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewNotification())
                {

                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateNotification();

        }

        return false;
    }
    public bool Delete()
    {
        return clsNotificationData.DeleteNotification(this.NotificationID); 
    }
    public static bool IsNotificationExist(int NotificationID)
    {
        return clsNotificationData.IsNotificationExist(NotificationID); 
    }
    public static DataTable GetAllNotification()
    {
        return clsNotificationData.GetAllNotification();

    }

    }
}