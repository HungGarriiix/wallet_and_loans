using Microsoft.AspNetCore.Mvc;
using wallet_and_loans_api.Common;
using wallet_and_loans_api.Common.Session;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IServices;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Services
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly IWalletBLO _walletBLO;
        private readonly IUserBLO _userBLO;

        public WalletService(IWalletBLO walletBLO, IUserBLO userBLO, ISessionDataProvider sessionDataProvider)
            : base(sessionDataProvider)
        {
            _walletBLO = walletBLO;
            _userBLO = userBLO;
        }

        public IEnumerable<WalletResponseDTO> GetWallets()
        {
            IEnumerable<Wallet> wallets = _walletBLO.GetWallets(_userBLO.GetCurrentUser());
            return BundleWalletsIntoList(wallets);
        }

        public WalletResponseDTO GetWalletByID(int id)
        {
            Wallet wallet = _walletBLO.GetWallet(id, _userBLO.GetCurrentUser());

            return new WalletResponseDTO(wallet);
        }

        public WalletResponseDTO AddWallet(CreateWalletDTO dto)
        {
            Wallet wallet = _walletBLO.CreateWallet(dto, _userBLO.GetCurrentUser());

            return new WalletResponseDTO(wallet);
        }

        public WalletResponseDTO UpdateWallet(int id, UpdateWalletDTO dto)
        {
            // throws "Wallet #id not found." when the wallet isn't the caller's
            _walletBLO.GetWallet(id, _userBLO.GetCurrentUser());
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
