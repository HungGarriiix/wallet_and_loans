using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IServices
{
    public interface IWalletService
    {
        IEnumerable<Wallet> GetWallets();
        Wallet GetWalletByID(int id);
        void AddWallet(CreateWalletDTO dto, out Wallet result);
        void UpdateWallet(int id, UpdateWalletDTO dto, out Wallet result);
    }
}
