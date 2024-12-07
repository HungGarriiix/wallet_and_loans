using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IServices
{
    public interface IWalletService
    {
        void AddWallet(CreateWalletDTO dto, out Wallet result);
    }
}
