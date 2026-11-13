using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IBLO
{
    public interface IAuthBLO
    {
        string GenerateToken(string userId);
        User GetUserInfo(string userId, string platform);
    }
}
