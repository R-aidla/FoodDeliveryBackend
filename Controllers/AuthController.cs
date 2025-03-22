using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Controllers
{
    /// <summary>
    /// Controls auth API calls.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    // TODO: Context is unused. Maybe have the database register and save the users?
    public class AuthController(AppDbContext context) : ControllerBase
    {
        // POST: api/auth/login
        /// <summary>
        /// Using the key given by the user, check it and save it in a cookie if valid.
        /// </summary>
        [HttpPost("login")]
        public ActionResult<IEnumerable<Company>> Login([FromBody] string? admin_key)
        {
            var config = ServiceLocator.GetService<IConfiguration>();

            if (admin_key == null || admin_key == string.Empty)
            {
                return BadRequest("No secret key inserted.");
            }

            if (admin_key == config["Secrets:AdminKey"])
            {
                bool.TryParse(config["ServerSettings:Secure"], out bool secure);

                /*Response.Cookies.Append("AdminKey", admin_key, new CookieOptions
                {
                    Secure = secure,
                    Expires = DateTime.UtcNow.AddHours(1)
                });*/

                return Ok("Login successful! Inserted key matches secret.");
            }
            else
            {
                return Unauthorized("Invalid secret key!");
            }
        }

        // DELETE: api/auth/logout
        /// <summary>
        /// Logs out the user by deleting the auth cookie.
        /// </summary>
        /*[HttpDelete("logout")]
        public ActionResult<IEnumerable<Company>> Logout()
        {
            Response.Cookies.Delete("AdminKey");
            return Ok(new { Message = "Logged out, key deleted from cookie" });
        }*/
    }
}
