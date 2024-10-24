﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class Debt
    {
        public Debt(Debtor loaned, string description)
        {
            Loaned = loaned;
            Description = description;
        }

        // get from db
        public Debt(Debtor loaned, string description, List<DebtItem> items, bool isPaid)
            : this(loaned, description) 
        {
            ItemList = items;
            IsPaid = isPaid;
        }

        public Debtor Loaned { get; set; } = new Debtor(0, string.Empty);
        public List<DebtItem> ItemList { get; set; } = new List<DebtItem>();
        public float MoneySpent { get { return ItemList.Sum(x => x.TotalPrice); } }
        public string Description { get; set; } = string.Empty;
        public int NumberOfItems { get { return ItemList.Count; } }
        public int NumberOfQuantities { get { return ItemList.Sum(x => x.Quantity); } }
        public bool IsPaid { get; private set; } = false;

        public void AddItem(DebtItem item)
        {
            CheckPaid();

            try
            {
                DebtItem found_item = GetItem(item.Name);
                if (found_item != null)             // if item is added, add its quantity instead
                {
                    found_item.Quantity += item.Quantity;
                    return;
                }
            }
            catch (Exception ex)                    // if item is not added, add it
            {
                ItemList.Add(item);
            }
        }

        public DebtItem GetItem(string name)
        {
            DebtItem item = ItemList.FirstOrDefault(x => x.Name == name);

            if (item == null) 
                throw new Exception($"Cannot find item with name '{name}'...");

            return item;
        }

        public DebtItem ChangeItemInfo(string name, DebtItem updated_item)
        {
            CheckPaid();
            DebtItem item = GetItem(name);

            item.Name = updated_item.Name;
            item.Quantity = updated_item.Quantity;
            item.SinglePrice = updated_item.SinglePrice;

            return item;
        }

        public void RemoveItem(string name, int quantity = 0)
        {
            CheckPaid();
            DebtItem item = GetItem(name); // assuming there is item in the list

            int quantity_check = (quantity == 0) ? 0 : item.Quantity - quantity;
            if (quantity_check > 0)
                item.Quantity = quantity_check;
            if (quantity_check == 0)        // if the user decides to delete the entire stock by providing
                ItemList.Remove(item);      // enough quantity or not at all
            if (quantity_check < 0)
                throw new Exception($"Cannot send {item.Name} below the ground!");
        }

        public void SealLoan()
        {
            IsPaid = true;
        }

        private void CheckPaid()
        {
            if (IsPaid)
                throw new Exception("The spending has been sealed.");
        }
    }
}
