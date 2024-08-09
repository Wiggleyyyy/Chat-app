using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddFriendController : ControllerBase
    {
        private database DB = new database();

        [HttpPost(Name = "Send_friend_request")]
        public async Task<IActionResult> Post([FromQuery] string self_user_tag, [FromQuery] string other_user_tag)
        {
            if (String.IsNullOrWhiteSpace(self_user_tag) || String.IsNullOrEmpty(other_user_tag))
            {
                return BadRequest("Missing or invalid data");
            }

            bool friendRequestExists = DB.checkForFriendRequest(self_user_tag, other_user_tag);

            if (friendRequestExists)
            {
                //Accept instead of send
            }
            else
            {
                bool isSuccess = DB.sendFriendRequst(self_user_tag, other_user_tag);

                if (!isSuccess)
                {
                    Console.WriteLine("Error creating friend request");
                    return BadRequest("Couldnt send friend request.");
                }

                return StatusCode(200, "Friend request sent.");
            }
        }
    }
}