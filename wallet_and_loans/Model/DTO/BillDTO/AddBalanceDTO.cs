namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class AddBalanceDTO
    {
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public int WalletUsedID { get; set; }
        public float Amount { get; set; }
    }
}
