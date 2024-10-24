using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wallet_and_loans.Logics;

namespace wallet_and_loans.DAO
{
    public interface PersonDAO
    {
        public void AddPerson(Debtor person);
        public Debtor GetPersonByID(int id);
        public Debtor GetPersonByName(string name);
        public void UpdatePerson(Debtor person);
        public void DeletePerson(int id);
    }
}
