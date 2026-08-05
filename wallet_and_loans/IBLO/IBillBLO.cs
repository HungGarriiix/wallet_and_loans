using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IBLO
{
    public interface IBillBLO
    {
        public IEnumerable<Bill> GetBills(User user);
        public Bill GetBillByID(int id);
        public Bill CreateBill(CreateBillDTO data, User user);
        public void AddItemToBill(Bill bill, BillItem item, ref float expectedBalance);
        public void UpdateBill(int targetBillId, Bill updateBill, ref Bill result);
    }
}
