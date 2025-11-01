using System.Reflection.Metadata.Ecma335;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_components.Logics;

namespace wallet_and_loans_api.BLO
{
    public class BillBLO: IBillBLO
    {
        private readonly IBillRepository _billRepository;
        private readonly IWalletBLO _walletBLO;
        public BillBLO(IBillRepository billRepository, IWalletBLO walletBLO)
        {
            this._billRepository = billRepository;
            this._walletBLO = walletBLO;
        }

        public IEnumerable<Bill> GetBills()
        {
            List<Bill> bills = _billRepository.GetBills();
            return bills;
        }

        public Bill GetBillByID(int id)
        {
            Bill bill = _billRepository.GetBill(id);
            if (bill == null)
            {
                throw new ArgumentException("Bill not found");
            }
            return bill;
        }

        public Bill CreateBill(CreateBillDTO data)
        {
            Wallet wallet = _walletBLO.GetWallet(data.WalletUsedID);

            int billCount = _billRepository.GetBillCount();
            Bill bill = new Bill(
                billCount,
                data.DateCreated,
                data.Description,
                wallet,
                TestStatic.UserTest
            );
            _billRepository.AddBill(bill);
            return bill;
        }

        public void AddItemToBill(Bill bill, BillItem item, ref float expectedBalance)
        {
            bill.AddItemToBill(item);
            expectedBalance = bill.WalletUsed.Balance - item.TotalPrice;
            _billRepository.UpdateBill(bill);
            _walletBLO.UpdateWallet(bill.WalletUsed.ID, new Model.DTO.WalletDTO.UpdateWalletDTO
            {
                Balance = expectedBalance
            });
        }
    }
}
