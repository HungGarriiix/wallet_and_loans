using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class BillDebt
    {
        public BillDebt(Bill bill)
        {
            BillToLoan = bill;
        }

        // get from db
        public BillDebt(Bill bill, List<Debt> loans, bool paymentCompleted)
            : this(bill)
        {
            Loans = loans;
            PaymentCompleted = paymentCompleted;
        }

        public Bill BillToLoan { get; private set; } = null;
        public List<Debt> Loans { get; private set; } = new List<Debt>();
        public bool PaymentCompleted { get; private set; } = false;

        public void AddLoan(Debt loan)
        {
            if (Loans.FirstOrDefault(s => s.Loaned == loan.Loaned) != null) // loaned person exists
                throw new Exception($"{loan.Loaned.Name} is already exists.");

            Loans.Add(loan);
        }

        public Debt GetLoan(Debtor person)
        {
            Debt spending = Loans.FirstOrDefault(s => s.Loaned == person);
            if (spending == null)
                throw new Exception($"Cannot find the details about {person.Name}'s loan.");

            return spending;
        }

        public void DeleteLoan(Debtor person)
        {
            Debt loan = GetLoan(person);
            Loans.Remove(loan);
        }

        public void AddItemToLoaned(Debt loan, DebtItem item)
        {
            if (CheckItemAvailabilityInLoans(item))
                loan.GetItem(item.Name).Quantity += item.Quantity;
        }

        private bool CheckItemAvailabilityInLoans(DebtItem item)
        {
            int total = 0; // get from Bill
            int loaned = GetTotalLoanedItems(item.Name);
            int addition = item.Quantity;   // if the user is going to add items to loaned

            if (total >= loaned)
                throw new Exception("The item is already full.");
            if (total > loaned + addition)
                throw new Exception("Cannot add more items.");

            return true;
        }

        private int GetTotalLoanedItems(string itemName)
        {
            return Loans.Sum(x => x.GetItem(itemName).Quantity);
        }

        public void MarkLoanFilled()
        {
            if (PaymentCompleted)
                throw new Exception("Loan has been fulfilled already!");

            PaymentCompleted = true;
        }
    }
}
