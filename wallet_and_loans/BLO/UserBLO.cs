using wallet_and_loans_api.Common.Session;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.BLO
{
    public class UserBLO: IUserBLO
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionDataProvider _sessionDataProvider;
        public UserBLO(IUserRepository userRepository, ISessionDataProvider sessionDataProvider)
        {
            _userRepository = userRepository;
            _sessionDataProvider = sessionDataProvider;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        // the user behind the current request; bill and wallet lookups are scoped to them
        public User GetCurrentUser()
        {
            // unregistered users are rejected by [UserAuthorization] before reaching here
            User user = _userRepository.GetUserById(Convert.ToInt32(_sessionDataProvider.UserId));
            if (user == null) throw new Exception("User not found.");
            return user;
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
                    Username = displayName,
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
