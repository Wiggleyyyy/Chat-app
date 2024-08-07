using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private database DB = new database();

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> Post([FromBody] Chats chat, [FromQuery] int userId_1, [FromQuery] int userId_2)
        {
            if (userId_1 <= 0 || userId_2 <= 0 || String.IsNullOrEmpty(chat.text))
            {
                Console.WriteLine("invalid or missing data");
                return BadRequest("Invalid or missing data");
            }

            chat.created_at = DateTime.Now;

            bool isSuccess = DB.sendMessage(chat);

            if (!isSuccess)
            {
                Console.WriteLine("Error creating message");
                return BadRequest("Error occured while creating message");
            }
            else
            {
                Console.WriteLine("Success");
                return StatusCode(200, "Message send");
            }
        }

        [HttpGet(Name = "GetMessage")]
        public async IEnumerable<ChatReturnData> Get([FromQuery] int userId_1, [FromQuery] int userId_2)
        {
            if (userId_1 <= 0 || userId_2 <= 0)
            {
                Console.WriteLine("Invalid or missing data");
                return BadRequest("Invalid or missing data");
            }

            List<Chats> messagesBetweenUsers = new List<Chats>();
            messagesBetweenUsers = DB.getMessagesBetweenUsers(userId_1, userId_2);

            if (messagesBetweenUsers.Count! > 0)
            {
                Console.WriteLine("couldnt find messages between users or none are made");
                return StatusCode(404, "Couldnt fint messages, or none are sent");
            }

            List<Chats> messagesFromUser_1 = messagesBetweenUsers.Where(x => x.from_user_id == userId_1).ToList();
            List<Chats> messagesFromUser_2 = messagesBetweenUsers.Where(x => x.from_user_id == userId_2).ToList();

            ChatReturnData dataToReturn = new ChatReturnData
            {
                chats_from_user_1 = messagesFromUser_1,
                chats_from_user_2 = messagesFromUser_2,
                user_id_1 = userId_1,
                user_id_2 = userId_2,
            };

            if (dataToReturn == null)
            {
                return BadRequest("Unkown error occured");
            }

            return dataToReturn;
        }
    }
}