using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class LoanItem: Item
    {
        public LoanItem(string name, int quantity, float singleValue)
            : base(name, quantity)
        {
            SinglePrice = singleValue;
        }

        public override float SinglePrice { get; set; }
        public override float TotalPrice { get => SinglePrice * Quantity; set => throw new NotImplementedException(); } 
        // only get is allowed on TotalPrice
    }
}
