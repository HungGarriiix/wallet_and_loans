using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.BLO
{
    public class WalletBLO: IWalletBLO
    {
        private readonly IWalletRepository _walletRepository;

        public WalletBLO(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public IEnumerable<Wallet> GetWallets()
        {
            return TestStatic.UserTest.Wallets;
        }

        public Wallet GetWallet(int id)
        {
            Wallet wallet = _walletRepository.GetWallet(id);
            if (wallet == null)
                throw new Exception("Wallet not found.");

            return wallet;
        }

        public Wallet CreateWallet(CreateWalletDTO data)
        {
            Wallet wallet = new Wallet
            {
                Name = data.Name,
                Balance = data.Balance,
            };

            wallet = _walletRepository.AddWallet(wallet);
            return wallet;
        }

        public Wallet UpdateWallet(int id, UpdateWalletDTO data)
        {
            if (data == null)
                throw new Exception("Wallet cannot be null.");
            Wallet wallet = _walletRepository.GetWallet(id);
            if (wallet == null)
                throw new Exception("Wallet not found.");

            wallet.Name = data.Name;
            wallet.Balance = data.Balance;
            return _walletRepository.UpdateWallet(wallet);
        }
    }
}
