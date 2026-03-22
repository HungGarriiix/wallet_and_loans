using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IRepositories
{
    public interface IUserRepository
    {
        User GetUserByProfile(string identifier, LoginPlatformEnum platform);
        User CreateUserByProfile(User user);
    }
}
