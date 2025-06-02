using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletBLO _walletBLO;
        public WalletService(IWalletBLO walletBLO)
        {
            _walletBLO = walletBLO;
        }

        public IEnumerable<WalletResponseDTO> GetWallets()
        {
            IEnumerable<Wallet> wallets = _walletBLO.GetWallets();
            return BundleWalletsIntoList(wallets);
        }

        public WalletResponseDTO GetWalletByID(int id)
        {
            Wallet wallet = _walletBLO.GetWallet(id);

            return new WalletResponseDTO(wallet);
        }

        public WalletResponseDTO AddWallet(CreateWalletDTO dto)
        {
            Wallet wallet = _walletBLO.CreateWallet(dto);

            return new WalletResponseDTO(wallet);
        }

        public WalletResponseDTO UpdateWallet(int id, UpdateWalletDTO dto)
        {
            Wallet wallet = _walletBLO.UpdateWallet(id, dto);

            return new WalletResponseDTO(wallet);
        }

        private List<WalletResponseDTO> BundleWalletsIntoList(IEnumerable<Wallet> wallets)
        {
            List<WalletResponseDTO> walletsList = new List<WalletResponseDTO>();
            foreach (Wallet wallet in wallets)
                walletsList.Add(new WalletResponseDTO(wallet));
            return walletsList;
        }
    }
}
