using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;

namespace wallet_and_loans_api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var user = _userService.GetUser();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}