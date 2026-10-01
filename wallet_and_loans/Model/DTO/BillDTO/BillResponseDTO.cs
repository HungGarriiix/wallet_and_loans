using wallet_and_loans_api.Model.DTO.ItemDTO;
using wallet_and_loans_components.Common.Enums;
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
            Type = bill.Type;
            TypeName = bill.Type.ToString();
        }

        public int ID { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public Wallet WalletUsedID { get; private set; }
        public string Owner { get; private set; }
        public float Total { get; set; }
        public BillType Type { get; private set; }
        public string TypeName { get; set; }
    }
}
