namespace wallet_and_loans_api.Model.DTO.WalletDTO
{
    public class WalletListResponseDTO
    {
        // set when there is something the user should be told, e.g. the list is empty
        public string? Message { get; set; }
        public IEnumerable<WalletResponseDTO> Wallets { get; set; } = new List<WalletResponseDTO>();
    }
}
