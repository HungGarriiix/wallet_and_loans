using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IBLO
{
    public interface IWalletBLO
    {
        public IEnumerable<Wallet> GetWallets();
        public Wallet GetWallet(int id);
        public Wallet CreateWallet(CreateWalletDTO data);
        public Wallet UpdateWallet(int id, UpdateWalletDTO data);
    }
}
