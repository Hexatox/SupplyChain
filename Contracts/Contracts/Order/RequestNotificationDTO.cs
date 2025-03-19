using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts.Order
{
    public class RequestNotificationDTO
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int SenderUserID { get; set; }
        public string ReceiverEmail { get; set; }

        public RequestNotificationDTO(string subject, string message, DateTime date, int senderUserID, string receiverEmail)
        {
            Subject = subject;
            Message = message;
            Date = date;
            SenderUserID = senderUserID;
            ReceiverEmail = receiverEmail;
        }


    }
}
