using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class LoanBill
    {
        public LoanBill(Bill bill)
        {
            BillToLoan = bill;
        }

        // get from db
        public LoanBill(Bill bill, List<Loan> loans, bool paymentCompleted)
            : this(bill)
        {
            Loans = loans;
            PaymentCompleted = paymentCompleted;
        }

        public Bill BillToLoan { get; private set; } = null;
        public List<Loan> Loans { get; private set; } = new List<Loan>();
        public bool PaymentCompleted { get; private set; } = false;

        public void AddLoan(Loan loan)
        {
            if (Loans.FirstOrDefault(s => s.Loaned == loan.Loaned) != null) // loaned person exists
                throw new Exception($"{loan.Loaned.Name} is already exists.");

            Loans.Add(loan);
        }

        public Loan GetLoan(Person person)
        {
            Loan spending = Loans.FirstOrDefault(s => s.Loaned == person);
            if (spending == null)
                throw new Exception($"Cannot find the details about {person.Name}'s loan.");

            return spending;
        }

        public void DeleteLoan(Person person)
        {
            Loan loan = GetLoan(person);
            Loans.Remove(loan);
        }

        public void AddItemToLoaned(Loan loan, LoanItem item)
        {
            if (CheckItemAvailabilityInLoans(item))
                loan.GetItem(item.Name).Quantity += item.Quantity;
        }

        private bool CheckItemAvailabilityInLoans(LoanItem item)
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
