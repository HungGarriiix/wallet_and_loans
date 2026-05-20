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

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByContact(string identifier, LoginPlatformEnum platform)
        {
            return _userRepository.GetUserByProfile(identifier, platform);
        }

        public User CreateNewUserByContact(string identifier, LoginPlatformEnum platform, string displayName)
        {
            User user = null;
            if (this.GetUserByContact(identifier, platform) == null)
            {
                TestStatic.UserCounter++;
                User newUser = new User()
                {
                    ID = TestStatic.UserCounter,
                    LoginProfiles = new List<LoginProfile>()
                    {
                        new LoginProfile()
                        {
                            Id = identifier,
                            Platform = platform,
                            ProfileName = displayName
                        }
                    }
                };
                user = _userRepository.CreateUserByProfile(newUser);
            }

            return user;
        }
    }
}
