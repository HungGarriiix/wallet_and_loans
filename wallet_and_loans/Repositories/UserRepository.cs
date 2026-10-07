using AutoMapper;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Repositories
{
    public class UserRepository: IUserRepository
    {
        public UserRepository() { }

        public User GetUserById(int id)
        {
            return TestStatic.Users.FirstOrDefault(u => u.ID == id);
        }

        public User GetUserByProfile(string identifier, LoginPlatformEnum platform)
        {
            return TestStatic.Users.FirstOrDefault(u =>
                u.LoginProfiles.Any(lp => lp.Id == identifier && lp.Platform == platform));
        }

        public User CreateUserByProfile(User user)
        {
            TestStatic.Users.Add(user);
            return user;
        }
    }
}
