using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public string Subject { get; set; }
        public string Message { get;set; }
        public DateTime Date { get;set; }
        public int SenderUserID { get; set; }
        public int ReceiverUserID {  get; set; }

        public NotificationDTO(int notificationID, string subject, string message, DateTime date, int senderUserID, int ReceiverUserID)
        {
            NotificationID = notificationID;
            Subject = subject;
            Message = message;
            Date = date;
            SenderUserID = senderUserID;
            this.ReceiverUserID = ReceiverUserID;
        }
   
    
    
    }
}
