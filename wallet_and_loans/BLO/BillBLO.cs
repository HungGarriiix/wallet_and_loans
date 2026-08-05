using System.Reflection.Metadata.Ecma335;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.WalletDTO;
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

        public IEnumerable<Bill> GetBills(User user)
        {
            List<Bill> bills = _billRepository.GetBills(user);
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

        public Bill CreateBill(CreateBillDTO data, User user)
        {
            Wallet wallet = _walletBLO.GetWallet(data.WalletUsedID);

            int billCount = _billRepository.GetBillCount();
            Bill bill = new Bill(
                billCount,
                data.DateCreated,
                data.Description,
                wallet,
                user
            );
            _billRepository.AddBill(bill);
            return bill;
        }

        public void AddItemToBill(Bill bill, BillItem item, ref float expectedBalance)
        {
            bill.AddItemToBill(item);
            expectedBalance = bill.WalletUsed.Balance - item.TotalPrice;
            _billRepository.UpdateBill(bill);
            bill.WalletUsed.Balance = expectedBalance;
            UpdateWalletDTO wallet = new UpdateWalletDTO
            {
                Name = bill.WalletUsed.Name,
                Balance = expectedBalance
            };
            _walletBLO.UpdateWallet(bill.WalletUsed.ID, wallet);
        }

        public void UpdateBill(int targetBillId, Bill updateBill, ref Bill result)
        {
            Bill targetBill = _billRepository.GetBill(targetBillId);
            if (targetBill == null)
            {
                throw new ArgumentException("Bill not found");
            }

            targetBill.Date = updateBill.Date;
            targetBill.Description = updateBill.Description;

            // return money to the wallet used in the original bill
            float originalBillTotal = targetBill.Total;
            updateBill.WalletUsed.Balance -= originalBillTotal;
            targetBill.WalletUsed.Balance += originalBillTotal;

            _walletBLO.UpdateWallet(targetBill.WalletUsed.ID, new UpdateWalletDTO
            {
                Name = targetBill.WalletUsed.Name,
                Balance = targetBill.WalletUsed.Balance
            });
            _walletBLO.UpdateWallet(updateBill.WalletUsed.ID, new UpdateWalletDTO
            {
                Name = updateBill.WalletUsed.Name,
                Balance = updateBill.WalletUsed.Balance
            });

            // Update the new wallet
            targetBill.WalletUsed = updateBill.WalletUsed;

            _billRepository.UpdateBill(targetBill);
            result = targetBill;
        }
    }
}
