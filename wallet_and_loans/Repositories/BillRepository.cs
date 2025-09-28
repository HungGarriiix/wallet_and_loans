using wallet_and_loans_api.IRepositories;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.Repositories
{
    public class BillRepository : IBillRepository
    {
        public BillRepository()
        {

        }

        public List<Bill> GetBills()
        {
            return TestStatic.Bills;
        }

        public Bill GetBill(int id)
        {
            return TestStatic.Bills.FirstOrDefault(b => b.ID == id);
        }

        public Bill AddBill(Bill bill)
        {
            if (bill == null)
                throw new Exception("Bill cannot be null.");
            int count = TestStatic.Bills.Count;
            if (TestStatic.Bills.Any(b => b.ID == count + 1))
                throw new Exception("Bill with this ID already exists.");

            bill.ID = count + 1;
            TestStatic.Bills.Add(bill);
            return bill;
        }

        public int GetBillCount()
        {
            return TestStatic.Bills.Count;
        }

        public void UpdateBill(Bill bill)
        {
            if (bill == null)
                throw new Exception("Bill cannot be null.");
            if (!TestStatic.Bills.Any(b => b.ID == bill.ID))
                throw new Exception("Bill with this ID does not exist.");

            int index = TestStatic.Bills.FindIndex(b => b.ID == bill.ID);
            TestStatic.Bills[index] = bill; // replace bill
        }
    }
}
