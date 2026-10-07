using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class UpdateBillDTO
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int WalletUsedId { get; set; }
    }
}
