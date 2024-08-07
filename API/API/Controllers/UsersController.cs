using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certifi
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private database DB = new database();

        [HttpPost(Name = "SignUp")]
        public async Task<IActionResult> Post([FromBody] Users user)
        {
            //Console.WriteLine(username);
            //return StatusCode(200, "test")

            user.created_at = DateTime.Now;

            if (String.IsNullOrEmpty(user.first_name) || String.IsNullOrEmpty(user.last_name) || String.IsNullOrEmpty(user.country) || String.IsNullOrEmpty(user.date_of_birth.ToString()) || String.IsNullOrEmpty(user.username) || String.IsNullOrEmpty(user.email) || String.IsNullOrEmpty(user.hashed_password) || String.IsNullOrEmpty(user.user_tag))
            {
                Console.WriteLine("missing data");
                return BadRequest("Invalid or missing data.");
            }

            bool signUpIsSuccess = DB.signUp(user);

            if (!signUpIsSuccess)
            {
                Console.WriteLine("couldnt create user");
                Console.WriteLine(user.ToString());
                return BadRequest("Couldn't create user.");
            }

            return StatusCode(200, "Created account.");
        }

        [HttpGet(Name = "SignIn")]
        public async IEnumerable<Users> Get([FromQuery] string username, [FromQuery] string hashed_password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(hashed_password))
            {
                Console.WriteLine("Missing or invalid data");
                return BadRequest("Missing or invalid data.");
            }

            }
        }
    }
}