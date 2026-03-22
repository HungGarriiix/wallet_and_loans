using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.BLO
{
    public class UserBLO: IUserBLO
    {
        private readonly IUserRepository _userRepository;
        public UserBLO(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public User GetUserByContact(string identifier, LoginPlatformEnum platform)
        {
            return _userRepository.GetUserByProfile(identifier, platform);
        }
    }
}
