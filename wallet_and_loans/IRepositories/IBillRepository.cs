using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.IRepositories
{
    public interface IBillRepository
    {
        List<Bill> GetBills(User user);
        Bill GetBill(int id);
        Bill AddBill(Bill bill);
        int GetBillCount();
        void UpdateBill(Bill bill);
    }
}
