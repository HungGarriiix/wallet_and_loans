using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace wallet_and_loans.Logics
{
    public class User : Person
    {
        public User(int id, string name, string username, string password)
            : base(id, name)
        {
            Username = username;
            Password = password;
        }

        // get from db
        public User(int id, string name, string username, string password, List<Contact> contacts, List<Person> loaned, List<Wallet> wallets)
            : base(id, name, contacts)
        {
            SavedLoanedList = loaned;
            Wallets = wallets;
            Username = username;
            Password = password;
        }

        public List<Person> SavedLoanedList { get; set; } = new List<Person>();
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public Person FindLoanedPerson(string name)
        {
            Person person = SavedLoanedList.FirstOrDefault(x => x.Name == name);
            if (person == null)
            {
                throw new Exception("Cannot find person with name " + name);
            }
            return person;
        }

        public Person FindLoanedPerson(int id)
        {
            Person person = SavedLoanedList.FirstOrDefault(x => x.ID == id);
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
    }
}
