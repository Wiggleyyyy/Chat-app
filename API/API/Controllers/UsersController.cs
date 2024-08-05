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
            if (String.IsNullOrEmpty(user.first_name) || String.IsNullOrEmpty(user.last_name) || String.IsNullOrEmpty(user.country) || String.IsNullOrEmpty(user.date_of_birth.ToString()) || String.IsNullOrEmpty(user.username) || String.IsNullOrEmpty(user.email) || String.IsNullOrEmpty(user.private_encrypted_password) || String.IsNullOrEmpty(user.user_tag))
            {
                return BadRequest("Invalid or missing data.");
            }

            bool signUpIsSuccess = DB.signUp(user);

            if (!signUpIsSuccess)
            {
                return BadRequest("Couldn't create user.");
            }

            return StatusCode(200, "Created account.");
        }

        [HttpGet(Name = "SignIn")]
        public IActionResult Get([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid or missing data.");
            }

            bool signInIsSuccess = DB.signIn(user);

            if (!signInIsSuccess)
            {
                return BadRequest("Couldn't create user.");
            }

            return StatusCode(200, "Logged in.");
        }
    }
}