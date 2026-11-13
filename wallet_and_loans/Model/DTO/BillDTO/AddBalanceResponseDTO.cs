namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class AddBalanceResponseDTO
    {
        public BillDetailsResponseDTO Bill { get; set; }
        public double ExpectedBalance { get; set; }
    }
}
