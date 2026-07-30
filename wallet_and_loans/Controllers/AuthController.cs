using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("register/third")]
        public IActionResult Register([FromBody] RegisterUserDTO dto)
        {
            try
            {
                bool isUserRegistered = _authService.CheckUserRegistered(dto.UserId, dto.PlatformId);
                if (isUserRegistered)
                {
                    return BadRequest("User has registered");
                }

                bool userCreated = _authService.RegisterNewUser(dto.UserId, dto.PlatformId, dto.DisplayName);
                if (userCreated)
                {
                    return Ok("User created");
                }
                else
                {
                    return BadRequest("Error");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
