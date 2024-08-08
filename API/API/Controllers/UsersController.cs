using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
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
        public async Task<ActionResult<IEnumerable<Users>>> Get([FromQuery] string username, [FromQuery] string hashed_password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(hashed_password))
            {
                Console.WriteLine("Missing or invalid data");
                return BadRequest("Missing or invalid data.");
            }

            try
            {
                Users user = DB.signIn(username, hashed_password);

                if (user == null)
                {
                    Console.WriteLine("Error getting user data");
                    return BadRequest("Error getting user data");
                }

                List<Users> userList = new List<Users> { user };
                return Ok(userList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}