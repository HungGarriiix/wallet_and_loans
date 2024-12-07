using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class WalletService: IWalletService
    {
        public WalletService() { }

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
    }
}
