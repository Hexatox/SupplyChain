using System;

public class NotificationRequestDTO
{
    public string Message { set; get; }
    public DateTime Date { set; get; }
    public int SenderUserID { set; get; }
    public string ReceiverEmail {  set; get; }
    public string Subject { get; set; }

    public NotificationRequestDTO(string Subject , string Message, DateTime Date, int SenderUserID,string ReceiverEmail)
	{
        this.Subject = Subject;
        this.Message = Message;
        this.Date = Date;
        this.SenderUserID = SenderUserID;
        this.ReceiverEmail = ReceiverEmail;
    }

}
