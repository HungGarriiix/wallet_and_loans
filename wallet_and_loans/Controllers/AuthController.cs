using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.AuthDTO;

namespace wallet_and_loans_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/login
        // Body: { "userName": "<discord_user_id>" }
        // Returns: { "token": "<jwt>" }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                var result = _authService.Login(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
