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
        public void AddPerson(Person person);
        public Person GetPersonByID(int id);
        public Person GetPersonByName(string name);
        public void UpdatePerson(Person person);
        public void DeletePerson(int id);
    }
}
