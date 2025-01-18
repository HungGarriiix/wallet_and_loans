using System.Diagnostics.Contracts;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Model.DTO.BillDTO
{
    public class CreateBillDTO
    {
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public int WalletUsedID { get; set; }
        public string Owner { get; set; }
    }
}
