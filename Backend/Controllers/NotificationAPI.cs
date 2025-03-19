using Business_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Notification")]
    [ApiController]
    public class NotificationAPI : ControllerBase
    {
        [HttpPost("SendMessage")]
        public ActionResult SendMessage(NotificationRequestDTO notificationRequestDTO)
        {
            if (notificationRequestDTO.Message == "" || notificationRequestDTO.Subject == "") {
                return BadRequest("Message/Subject should not be empty");
            }
            if (clsNotification.SendMessage(notificationRequestDTO))
            {
                return Ok("Message Sent Successfully");
            }
            return NotFound("Message delivery failed");
        }

    }
}
