using System;

public class NotificationResponseDTO
{
    public int NotificationID { set; get; }
    public string Message { set; get; }
    public DateTime Date { set; get; }
    public int UserID { set; get; }

    public NotificationResponseDTO(int NotificationID, string Message, DateTime Date, int UserID)
    {
        this.NotificationID = NotificationID;
        this.Message = Message;
        this.Date = Date;
        this.UserID = UserID;
    }
}
