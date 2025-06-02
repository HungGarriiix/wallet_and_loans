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
            return TestStatic.UserTest.Wallets;
        }

        public Wallet GetWallet(int id)
        {
            return TestStatic.UserTest.Wallets.FirstOrDefault(w => w.ID == id);
        }

        public Wallet AddWallet(Wallet wallet)
        {
            if (wallet == null)
                throw new Exception("Wallet cannot be null.");
            int count = TestStatic.UserTest.Wallets.Count;
            if (TestStatic.UserTest.Wallets.Any(w => w.ID == count + 1))
                throw new Exception("Wallet with this ID already exists.");
            
            wallet.ID = count + 1;
            TestStatic.UserTest.AddWallet(wallet);
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
