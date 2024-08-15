using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans_test
{
    public class LoanItemTest
    {
        LoanItem loan1;

        [SetUp]
        public void Setup()
        {
            loan1 = new LoanItem("Test", 4, 12.00f);
        }

        [Test]
        public void TestLoanTotalPrice()
        {
            float expected = 12.00f * 4;
            Assert.AreEqual(expected, loan1.TotalPrice);
        }

        [Test]
        public void TestSettingTotalPriceNotImplemented()
        {
            Assert.Throws<NotImplementedException>(() => loan1.TotalPrice = 100);
        }
    }
}
