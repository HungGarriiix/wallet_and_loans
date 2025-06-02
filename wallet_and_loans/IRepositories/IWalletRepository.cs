using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IRepositories
{
    public interface IWalletRepository
    {
        public List<Wallet> GetAllWallets();
        public Wallet GetWallet(int id);
        public Wallet AddWallet(Wallet wallet);
        public Wallet UpdateWallet(Wallet wallet);
    }
}
