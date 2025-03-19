using Business_Layer;
using Contracts.Contracts;
using Contracts.Contracts.Order;
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



        [HttpGet("GetMessagesByUserID/{UserID}", Name = "GetMessagesByUserID")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<List<NotificationDTO>>> GetMessagesByUserID(int UserID)
        {
            if (UserID < 0)
            {
                return BadRequest("UserID should be greater than 0");
            }
            List<NotificationDTO> result = await clsNotification.GetMessagesByUserID(UserID);
            if (result == null) return NotFound("No messages found!");
            return Ok(result);
        }

    }
}
