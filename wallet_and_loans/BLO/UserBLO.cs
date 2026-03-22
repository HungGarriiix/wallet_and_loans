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

        public User CreateNewUserByContact(string identifier, LoginPlatformEnum platform)
        {
            User user = null;
            if (this.GetUserByContact(identifier, platform) == null)
            {
                int seed = TestStatic.Users.Count + 1;
                User newUser = new User()
                {
                    ID = seed,
                    LoginProfiles = new List<LoginProfile>()
                    {
                        new LoginProfile()
                        {
                            Id = identifier,
                            Platform = platform,
                        }
                    }
                };
                user = _userRepository.CreateUserByProfile(newUser);
            }

            return user;
        }
    }
}
