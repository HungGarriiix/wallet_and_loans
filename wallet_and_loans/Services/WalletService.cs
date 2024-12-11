using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class WalletService: IWalletService
    {
        public WalletService() { }

        public IEnumerable<Wallet> GetWallets()
        {
            return TestStatic.UserTest.Wallets;
        }

        public Wallet GetWalletByID(int id)
        {
            Wallet wallet = TestStatic.UserTest.Wallets.FirstOrDefault(x => x.ID == id);
            if (wallet == null)
                throw new Exception("Wallet not found.");
            else return wallet;
        }

        public void AddWallet(CreateWalletDTO dto, out Wallet result)
        {
            Wallet wallet = new Wallet
            {
                ID = TestStatic.UserTest.Wallets.Count + 1,
                Name = dto.Name,
                Balance = dto.Balance,
            };
            TestStatic.UserTest.AddWallet(wallet);
            result = wallet;
        }

        public void UpdateWallet(int id, UpdateWalletDTO dto, out Wallet result)
        {
            Wallet wallet = TestStatic.UserTest.Wallets.FirstOrDefault(x => x.ID == id);
            if (wallet == null)
                throw new Exception("Wallet not found.");
            
            wallet.Name = dto.Name;
            wallet.Balance = dto.Balance;

            result = wallet;
        }
    }
}
