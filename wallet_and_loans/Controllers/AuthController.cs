using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace wallet_and_loans_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok("Login successful");
        }
    }
}
