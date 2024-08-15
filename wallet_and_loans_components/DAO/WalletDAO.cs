using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wallet_and_loans.Logics;

namespace wallet_and_loans.DAO
{
    public interface WalletDAO
    {
        public void AddWallet(User user, Wallet wallet);
        public List<Wallet> GetWallets(User user);
        public Wallet GetWalletByName(User user, string name);
        public void UpdateWallet(User user, Wallet wallet);
        public void DeleteWallet(User user, Wallet wallet);
    }
}
