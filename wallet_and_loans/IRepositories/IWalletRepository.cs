using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IRepositories
{
    public interface IWalletRepository
    {
        public List<Wallet> GetAllWallets();
        public List<Wallet> GetWallets(User user);
        public Wallet GetWallet(int id);
        public Wallet AddWallet(Wallet wallet, User user);
        public Wallet UpdateWallet(Wallet wallet);
    }
}
