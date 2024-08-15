using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wallet_and_loans.Logics;

namespace wallet_and_loans.DAO
{
    public interface IBillDAO
    {
        public void AddBill(Bill Bill);
        public List<Bill> GetBillsByUser(User user);
        public List<Bill> GetUserBillsByFilter(User user, BillFilter filter);
        public Bill GetBillByID(User user, int id);
        public void UpdateBill(Bill Bill);
        public void DeleteBill(Bill Bill);
    }
}
