using System.Reflection.Metadata.Ecma335;
using wallet_and_loans_api.IBLO;
using wallet_and_loans_api.IRepositories;
using wallet_and_loans_api.Model.DTO.BillDTO;
using wallet_and_loans_api.Model.DTO.WalletDTO;
using wallet_and_loans_components.Common.Enums;
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
            bill.Type = BillType.EXPENSE;
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

        public void DeleteItemFromBill(int billId, int itemIndex)
        {
            Bill bill = _billRepository.GetBill(billId);
            if (bill == null)
            {
                throw new ArgumentException("Bill not found");
            }
            if (itemIndex < 0 || itemIndex >= bill.Items.Count)
            {
                throw new ArgumentOutOfRangeException("Item index is out of range");
            }
            BillItem itemToRemove = bill.Items[itemIndex];
            float expectedBalance = bill.WalletUsed.Balance + itemToRemove.TotalPrice;
            bill.RemoveItemFromBill(itemToRemove.Name);
            _billRepository.UpdateBill(bill);
            // Update the wallet balance
            bill.WalletUsed.Balance = expectedBalance;
            UpdateWalletDTO wallet = new UpdateWalletDTO
            {
                Name = bill.WalletUsed.Name,
                Balance = expectedBalance
            };
            _walletBLO.UpdateWallet(bill.WalletUsed.ID, wallet);
        }

        public Bill DeleteAllBillItems(int billId)
        {
            Bill bill = _billRepository.GetBill(billId);
            if (bill == null)
            {
                throw new ArgumentException("Bill not found");
            }
            float expectedBalance = bill.WalletUsed.Balance + bill.Total;
            bill.ClearAllItems();
            _billRepository.UpdateBill(bill);
            // Update the wallet balance
            bill.WalletUsed.Balance = expectedBalance;
            UpdateWalletDTO wallet = new UpdateWalletDTO
            {
                Name = bill.WalletUsed.Name,
                Balance = expectedBalance
            };
            _walletBLO.UpdateWallet(bill.WalletUsed.ID, wallet);
            return bill;
        }

        public Bill AddBalance(AddBalanceDTO data, User user, ref float expectedBalance)
        {
            if (data.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }
            Wallet wallet = _walletBLO.GetWallet(data.WalletUsedID);

            Bill bill = new Bill(
                _billRepository.GetBillCount(),
                data.DateCreated,
                data.Description,
                wallet,
                user
            );
            bill.Type = BillType.ADDITION;
            // the received amount is stored as a single item so it shows up in the bill history
            string itemName = string.IsNullOrWhiteSpace(data.Description) ? "Balance increment" : data.Description;
            bill.AddItemToBill(new BillItem(itemName, 1, data.Amount));
            _billRepository.AddBalanceBill(bill);

            // increase the wallet balance
            expectedBalance = wallet.Balance + data.Amount;
            wallet.Balance = expectedBalance;
            _walletBLO.UpdateWallet(wallet.ID, new UpdateWalletDTO
            {
                Name = wallet.Name,
                Balance = expectedBalance
            });
            return bill;
        }
    }
}
