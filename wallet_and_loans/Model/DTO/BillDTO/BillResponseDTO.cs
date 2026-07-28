using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class BillResponseDTO
    {
        public BillResponseDTO(Bill bill)
        {
            ID = bill.ID;
            Date = bill.Date;
            Description = bill.Description;
            WalletUsedID = bill.WalletUsed;
            Owner = bill.Owner.Username;
            Total = bill.Total;
        }

        public int ID { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public Wallet WalletUsedID { get; private set; }
        public string Owner { get; private set; }
        public float Total { get; set; }
    }
}
