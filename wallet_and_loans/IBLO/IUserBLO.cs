using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IBLO
{
    public interface IUserBLO
    {
        User GetUserById(int id);
        User GetCurrentUser();
        User GetUserByContact(string identifier, LoginPlatformEnum platform);
        User CreateNewUserByContact(string identifier, LoginPlatformEnum platform, string displayName);
    }
}
