using wallet_and_loans_api.Common;
using wallet_and_loans_api.Common.Session;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.UserDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserBLO _userBLO;

        public UserService(IUserBLO userBLO, ISessionDataProvider sessionDataProvider)
            : base(sessionDataProvider)
        {
            _userBLO = userBLO;
        }

        public bool CheckUserAvailability()
        {
            return _userBLO.GetUserById(Convert.ToInt32(_sessionDataProvider.UserId)) != null;
        }

        public UserResponseDTO GetUser()
        {
            User user = _userBLO.GetUserById(Convert.ToInt32(_sessionDataProvider.UserId));
            if (user == null)
                throw new Exception("User not found.");
            return new UserResponseDTO(user);
        }
    }
}