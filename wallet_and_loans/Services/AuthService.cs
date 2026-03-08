using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.AuthDTO;

namespace wallet_and_loans_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthBLO _authBLO;

        public AuthService(IAuthBLO authBLO)
        {
            _authBLO = authBLO;
        }

        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            string token = _authBLO.GenerateToken(dto.UserName);
            return new LoginResponseDTO { Token = token };
        }
    }
}
