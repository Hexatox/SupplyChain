using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Contracts.Contracts;
using DataAccess_Layer;

namespace Business_Layer{
    public class clsNotification
    {
        public NotificationDTO notificationDTO { get {
                return new NotificationDTO(
            NotificationID, Subject, Message, Date, SenderUserID, ReceiverUserID);
                    } }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int NotificationID {set;get;}
        public string Message {set;get;}
        public string Subject { get;set;}
        public DateTime Date {set;get;}
        public int SenderUserID {set;get;}
        public int ReceiverUserID { set; get; }
        public clsUser SenderUser {set;get;}
        public clsUser ReceiverUser { set; get; }
    private clsNotification (NotificationDTO notificationDTO , enMode mode = enMode.AddNew)
        {
        this.NotificationID = notificationDTO.NotificationID;
        this.Message = notificationDTO.Message;
        this.Date = notificationDTO.Date;
        this.SenderUserID = notificationDTO.SenderUserID;
        this.Subject = notificationDTO.Subject;
        this.ReceiverUserID = notificationDTO.ReceiverUserID;


        this.SenderUser = clsUser.Find(SenderUserID);
        this.ReceiverUser = clsUser.Find(ReceiverUserID);


            this.Mode = mode;
}
    private bool _AddNewNotification(){
        this.NotificationID = clsNotificationData.AddNewNotification(this.Message, this.Date, this.SenderUserID);
        return (this.NotificationID != -1);
    }
    private bool _UpdateNotification(){
        return clsNotificationData.UpdateNotification(this.NotificationID, this.Message, this.Date, this.SenderUserID);
    }
    //public static clsNotification Find(int NotificationID){
    //    string Message = "";
    //    DateTime Date = DateTime.Now;
    //    int UserID = -1;

    //    bool IsFound = clsNotificationData.GetNotificationInfoByNotificationID(
    //        NotificationID, ref Message, ref Date, ref UserID);

    //    if (IsFound){
    //        return new clsNotification(notificationDTO,enMode.Update);}
    //    else{ return null;}
    //}
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

    public static bool SendMessage(NotificationRequestDTO notificationRequest)
        {
            return clsNotificationData.SendMessage(notificationRequest);
        }
    }
}