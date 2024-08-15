using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class Wallet
    {
        public Wallet() 
        {

        }

        public int ID {  get; set; }
        public string Name { get; set; } = "";
        public float Balance { get; set; } = 0;

        public void AddBalance (float balance) 
        {
            Balance += balance;
        }

        public void DeductBalance(float balance) 
        {
            Balance -= balance;
        }
    }
}
