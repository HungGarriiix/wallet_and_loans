using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.WalletDTO
{
    public class WalletResponseDTO
    {
        public WalletResponseDTO(Wallet wallet)
        {
            ID = wallet.ID;
            Name = wallet.Name;
            Balance = wallet.Balance;
        }
        public int ID { get; }
        public string Name { get; }
        public float Balance { get; }
    }
}
