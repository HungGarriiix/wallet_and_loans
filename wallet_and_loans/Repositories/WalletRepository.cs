using wallet_and_loans_api.IRepositories;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Repositories
{
    public class WalletRepository: IWalletRepository
    {
        public WalletRepository()
        {

        }

        public List<Wallet> GetAllWallets()
        {
            List<Wallet> wallets = new List<Wallet>();
            foreach (User user in TestStatic.Users)
            {
                wallets.AddRange(user.Wallets);
            }
            return wallets;
        }

        public int GetAllWalletsCount()
        {
            return GetAllWallets().Count;
        }

        public List<Wallet> GetWallets(User user)
        {
            return user.Wallets;
        }

        public Wallet GetWallet(int id)
        {
            foreach (User user in TestStatic.Users)
            {
                Wallet wallet = user.Wallets.FirstOrDefault(w => w.ID == id);
                if (wallet != null)
                    return wallet;
            }
            return null;
        }

        public Wallet AddWallet(Wallet wallet, User user)
        {
            if (wallet == null)
                throw new Exception("Wallet cannot be null.");

            TestStatic.WalletCounter++;
            wallet.ID = TestStatic.WalletCounter;
            user.AddWallet(wallet);
            return wallet;
        }

        public Wallet UpdateWallet(Wallet wallet)
        {
            if (wallet == null)
                throw new Exception("Wallet cannot be null.");
            Wallet existingWallet = this.GetWallet(wallet.ID);
            if (existingWallet == null)
                throw new Exception("Wallet not found.");
            existingWallet.Name = wallet.Name;
            existingWallet.Balance = wallet.Balance;

            return existingWallet;
        }
    }
}
