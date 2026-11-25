using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IServices
{
    public interface IWalletService
    {
        WalletListResponseDTO GetWallets();
        WalletResponseDTO GetWalletByID(int id);
        WalletResponseDTO AddWallet(CreateWalletDTO dto);
        WalletResponseDTO UpdateWallet(int id, UpdateWalletDTO dto);
    }
}
