using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public struct BillFilter
    {
        public BillFilter()
        {
            Date = null;
            WalletUsed = null;
        }

        public DateTime? Date { get; set; }
        public Wallet? WalletUsed { get; set; }
    }
}
