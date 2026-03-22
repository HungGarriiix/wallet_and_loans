using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.AuthDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthBLO _authBLO;
        private readonly IUserBLO _userBLO;

        public AuthService(IAuthBLO authBLO, IUserBLO userBLO)
        {
            _authBLO = authBLO;
            _userBLO = userBLO;
        }

        public LoginResponseDTO Login(LoginRequestDTO dto)
        {
            string token = _authBLO.GenerateToken(dto.UserName);
            return new LoginResponseDTO { Token = token };
        }

        public bool CheckUserRegistered(string userId, int platform)
        {
            User user = _userBLO.GetUserByContact(userId, (LoginPlatformEnum)platform);
            return user != null;
        }

        public bool RegisterNewUser(string userId, int platform)
        {
            //User user = _userBLO.GetUserByContact(userId, (LoginPlatformEnum)platform);
            User user = _userBLO.CreateNewUserByContact(userId, (LoginPlatformEnum)platform);
            return user != null;
        }
    }
}
