using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace wallet_and_loans.Logics
{
    public class Bill
    {
        public Bill(int id, DateTime date, string description, Wallet wallet, User owner)
        {
            ID = id;
            Date = date;
            Description = description;
            WalletUsed = wallet; 
            Owner = owner;
        }

        public Bill(int id, DateTime date, string description, List<BillItem> items, Wallet wallet, User owner)
            : this(id, date, description, wallet, owner)
        {
            Items = items;
        }

        public int ID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;
        public List<BillItem> Items { get; private set; } = new List<BillItem>();
        public Wallet WalletUsed { get; set; } = null;  // might be changed to default wallet
        public User Owner { get; set; } = null;
        public float Total { get { return Items.Sum(item => item.TotalPrice); } }
        public int NumberOfItems { get { return Items.Count; } }
        public int NumberOfQuantities { get { return Items.Sum(x => x.Quantity); } }

        public void AddItemToBill(BillItem item)
        {
            try
            {
                BillItem found_item = GetItemFromBill(item.Name);
                if (found_item != null)             // if item is not added, add it
                {
                    found_item.Quantity += item.Quantity;
                    return;
                }
            }
            catch (Exception ex)
            {
                Items.Add(item);
            }
        }

        public void RemoveItemFromBill(string name, int quantity = 0)
        {
            BillItem item = GetItemFromBill(name); // assuming there is item in the list

            int quantity_check = (quantity == 0) ? 0 : item.Quantity - quantity;
            if (quantity_check > 0)
                item.Quantity = quantity_check;
            if (quantity_check == 0)        // if the user decides to delete the entire stock by providing
                Items.Remove(item);      // enough quantity or not at all
            if (quantity_check < 0)
                throw new Exception($"Cannot send {item.Name} below the ground!");
        }

        public BillItem GetItemFromBill(string name)
        {
            BillItem item = Items.Find(x => x.Name == name);
            if (item == null)
                throw new Exception($"Cannot find item with name '{name}'...");

            return item;
        }

        private void DeductWalletBalance()
        {
            WalletUsed.DeductBalance(Total);
        }
    }
}
