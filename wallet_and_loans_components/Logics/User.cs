using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace wallet_and_loans_components.Logics
{
    public class User
    {
        public User(int id, string username, string password)
        {
            ID = id;
            Username = username;
            Password = password;
        }

        // get from db
        public User(int id, string username, string password, List<Contact> contacts, List<Debtor> loaned, List<Wallet> wallets)
        {
            ID = id;
            SavedLoanedList = loaned;
            Wallets = wallets;
            Username = username;
            Password = password;
        }

        public int ID { get; set; }
        public List<LoginProfile> LoginProfiles { get; set; } = new List<LoginProfile>();
        public List<Debtor> SavedLoanedList { get; set; } = new List<Debtor>();
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public Debtor FindLoanedPerson(string name)
        {
            Debtor person = SavedLoanedList.FirstOrDefault(x => x.Name == name);
            if (person == null)
            {
                throw new Exception("Cannot find person with name " + name);
            }
            return person;
        }

        public Debtor FindLoanedPerson(int id)
        {
            Debtor person = SavedLoanedList.FirstOrDefault(x => x.ID == id);
            if (person == null)
            {
                throw new Exception("Cannot find person with ID " + id);
            }
            return person;
        }

        public Wallet GetWallet(int id) 
        {
            Wallet wallet = Wallets.FirstOrDefault(x => x.ID == id);
            if (wallet == null) 
            {
                throw new Exception("Cannot find wallet with ID " + id);
            }
            return wallet;
        }

        public Wallet GetWallet(string name)
        {
            Wallet wallet = Wallets.FirstOrDefault(x => x.Name == name);
            if (wallet == null)
            {
                throw new Exception("Cannot find wallet with name " + name);
            }
            return wallet;
        }

        public void AddWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
        }
    }
}
