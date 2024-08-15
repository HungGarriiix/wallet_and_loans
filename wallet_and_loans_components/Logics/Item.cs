using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public abstract class Item
    {
        public Item(string name, int quantity) 
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public abstract float TotalPrice { get; set; }
        public abstract float SinglePrice { get; set; }
        // When people assigning prices for the items, they tend to set the total of price written on the bill
        //, so only the total price is edittable
    }
}
