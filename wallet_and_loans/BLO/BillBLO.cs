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

        // another user's bill is reported as missing so its existence isn't revealed
        public Bill GetBillByID(int id, User user)
        {
            Bill bill = _billRepository.GetBill(id, user);
            if (bill == null)
            {
                throw new ArgumentException($"Bill #{id} not found.");
            }
            return bill;
        }

        public Bill CreateBill(CreateBillDTO data, User user)
        {
            Wallet wallet = _walletBLO.GetWallet(data.WalletUsedID, user);

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
            EnsureBillIsExpense(bill);
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

        public void UpdateBill(int targetBillId, UpdateBillDTO data, User user, ref Bill result)
        {
            Bill targetBill = GetBillByID(targetBillId, user);
            EnsureBillIsExpense(targetBill);
            Wallet newWallet = _walletBLO.GetWallet(data.WalletUsedId, user);

            targetBill.Date = data.Date;
            targetBill.Description = data.Description;

            // return money to the wallet used in the original bill
            float originalBillTotal = targetBill.Total;
            newWallet.Balance -= originalBillTotal;
            targetBill.WalletUsed.Balance += originalBillTotal;

            _walletBLO.UpdateWallet(targetBill.WalletUsed.ID, new UpdateWalletDTO
            {
                Name = targetBill.WalletUsed.Name,
                Balance = targetBill.WalletUsed.Balance
            });
            _walletBLO.UpdateWallet(newWallet.ID, new UpdateWalletDTO
            {
                Name = newWallet.Name,
                Balance = newWallet.Balance
            });

            // Update the new wallet
            targetBill.WalletUsed = newWallet;

            _billRepository.UpdateBill(targetBill);
            result = targetBill;
        }

        public void DeleteItemFromBill(int billId, int itemIndex, User user)
        {
            Bill bill = GetBillByID(billId, user);
            EnsureBillIsExpense(bill);
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

        public Bill DeleteAllBillItems(int billId, User user)
        {
            Bill bill = GetBillByID(billId, user);
            EnsureBillIsExpense(bill);
            EnsureBillHasItems(bill);
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
            Wallet wallet = _walletBLO.GetWallet(data.WalletUsedID, user);

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

        // item/wallet logic above assumes an expense, so ADDITION bills must not go through it
        private void EnsureBillIsExpense(Bill bill)
        {
            if (bill.Type == BillType.ADDITION)
            {
                throw new ArgumentException("Cannot modify a balance addition bill");
            }
        }

        // clearing a bill with no items would report success while nothing changed
        private void EnsureBillHasItems(Bill bill)
        {
            if (bill.Items.Count == 0)
            {
                throw new ArgumentException($"Bill #{bill.ID} has no items to clear.");
            }
        }
    }
}
