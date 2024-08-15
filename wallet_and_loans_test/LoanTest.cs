using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace wallet_and_loans_test
{
    public class LoanTest
    {
        Person person1;
        Loan loan1;
        LoanItem cart;
        LoanItem shoes;
        LoanItem chocolate;

        [SetUp]
        public void SetUp()
        {
            person1 = new Person(1, "Test");
            loan1 = new Loan(person1, "Test loaned.");
            cart = new LoanItem("Cart", 13, 15.00f);
            shoes = new LoanItem("Shoes", 1, 100f);
            chocolate = new LoanItem("Chocolate", 10, 1f);
        }

        [Test]
        public void TestSpendingDefaultConstructor()
        {
            Assert.Multiple(() =>
            {
                Assert.That(loan1.Loaned, Is.SameAs(person1));
                Assert.That(loan1.Description, Is.EqualTo("Test loaned."));
            });
        }

        [TestCaseSource("AddItemTestCase")] // might optimise this to accept more LoanStack objects
        public void TestAddItem((LoanItem item, string itemName) test)
        {
            loan1.AddItem(test.item);

            Assert.That(loan1.GetItem(test.itemName).Name, Is.EqualTo(test.itemName));
        }

        [Test]
        public void TestAddItemWhenPaid()
        {
            loan1.SealLoan();

            Assert.Throws<Exception>(() => loan1.AddItem(shoes), "The spending has been sealed.");
        }

        [Test]
        public void TestAddSameItem()
        {
            loan1.AddItem(shoes);
            loan1.AddItem(shoes);

            Assert.Multiple(() =>
            {
                Assert.That(loan1.GetItem("Shoes").Quantity, Is.EqualTo(2));
                Assert.That(loan1.NumberOfItems, Is.EqualTo(1));
            });
        }

        [TestCaseSource("AddItemTestCase")]
        public void TestGetItem((LoanItem item, string itemName) test)
        {
            loan1.ItemList.Add(test.item);

            Assert.That(loan1.GetItem(test.itemName), Is.SameAs(test.item));
        }

        [Test]
        public void TestGetNonExistedItem()
        {
            Assert.Throws<Exception>(() => loan1.GetItem("Cart"), "Cannot find item with name 'Cart' ...");
        }

        [Test]
        public void TestGetItemWhenPaid()
        {
            loan1.SealLoan();

            Assert.Throws<Exception>(() => loan1.GetItem("Random item"), "The spending has been sealed.");
        }

        [Test]
        public void TestChangeItemInfo()
        {
            loan1.AddItem(shoes);
            LoanItem test = new LoanItem("Test change", 15, 15f);
            loan1.ChangeItemInfo(shoes.Name, test);

            Assert.Multiple(() =>
            {
                Assert.That(loan1.GetItem("Test change").Name, Is.EqualTo("Test change"));
                Assert.That(loan1.GetItem("Test change").Quantity, Is.EqualTo(15));
                Assert.That(loan1.GetItem("Test change").SinglePrice, Is.EqualTo(15f));
                Assert.That(loan1.GetItem("Test change"), Is.Not.SameAs(test));
            });
        }

        [Test]
        public void TestChangeNonExistedItem()
        {
            Assert.Throws<Exception>(() => loan1.ChangeItemInfo("Non existed", new LoanItem("Random item", 1, 1f)), "Cannot find item with name 'Non existed'...");
        }

        [Test]
        public void TestChangeItemInfoWhenSealed()
        {
            loan1.SealLoan();

            Assert.Throws<Exception>(() => loan1.ChangeItemInfo("Random item", shoes), "The spending has been sealed.");
        }

        [Test]
        public void TestRemoveItem()
        {
            loan1.AddItem(shoes);
            loan1.AddItem(cart);
            loan1.AddItem(chocolate);
            loan1.RemoveItem(shoes.Name);
            loan1.RemoveItem(cart.Name);
            loan1.RemoveItem(chocolate.Name);

            Assert.Multiple(() =>
            {
                Assert.Throws<Exception>(() => loan1.GetItem(shoes.Name));
                Assert.Throws<Exception>(() => loan1.GetItem(cart.Name));
                Assert.Throws<Exception>(() => loan1.GetItem(chocolate.Name));
            });
        }

        [TestCase("Testing")]
        [TestCase("Shoes")]
        [TestCase("Cart")]
        [TestCase("Chocolate")]
        public void TestRemoveNonExistedItem(string item)
        {
            Assert.Throws<Exception>(() => loan1.RemoveItem(item), $"Cannot find item with name '{item}'...");
        }

        [Test]
        public void TestRemoveASetOfQuantity()
        {
            loan1.AddItem(chocolate);
            loan1.AddItem(cart);
            loan1.RemoveItem(chocolate.Name, 4);

            Assert.Multiple(() =>
            {
                Assert.That(chocolate.Quantity, Is.EqualTo(6));
                Assert.That(cart.Quantity, Is.EqualTo(13));
            });
        }

        [Test]
        public void TestRemoveEntireQuantity()
        {
            loan1.AddItem(chocolate);
            loan1.RemoveItem(chocolate.Name, 10);

            Assert.Throws<Exception>(() => loan1.GetItem("Chocolate"), "Cannot find item with name 'Chocolate'...");
        }

        [Test]
        public void TestRemoveItemWhenSealed()
        {
            loan1.SealLoan();

            Assert.Throws<Exception>(() => loan1.RemoveItem("Random item"), "The spending has been sealed.");
        }

        [Test]
        public void TestSealLoan()
        {
            loan1.SealLoan();

            Assert.That(loan1.IsPaid, Is.True);
        }

        [TestCaseSource("AddItemTestCasePrice")]    // might optimise this to accept more LoanStack objects
        public void TestNumberOfItemsWhenAdd1((LoanItem item, int quantity) i)
        {
            loan1.AddItem(i.item);

            Assert.That(loan1.NumberOfItems, Is.EqualTo(1));
        }

        [Test]
        public void TestNumberOfItemsWhenAddAll()
        {
            loan1.AddItem(shoes);
            loan1.AddItem(cart);
            loan1.AddItem(chocolate);

            Assert.That(loan1.NumberOfItems, Is.EqualTo(3));
        }

        [Test]
        public void TestNumberOfItemsWhenEmpty()
        {
            Assert.That(loan1.NumberOfItems, Is.EqualTo(0));
        }

        [TestCaseSource("AddItemTestCasePrice")]    // might optimise this to accept more LoanStack objects
        public void TestNumberOfQuantitiesWhenAddOne((LoanItem item, int quantity) i)
        {
            loan1.AddItem(i.item);

            Assert.That(loan1.NumberOfQuantities, Is.EqualTo(i.quantity));
        }

        [Test]
        public void TestNumberOfQuantitiesWhenAddAll()
        {
            loan1.AddItem(shoes);
            loan1.AddItem(cart);
            loan1.AddItem(chocolate);

            Assert.That(loan1.NumberOfQuantities, Is.EqualTo(24));
        }

        [Test]
        public void TestNumberOfQuantitiesWhenEmpty()
        {
            Assert.That(loan1.NumberOfQuantities, Is.EqualTo(0));
        }

        private static IEnumerable<(LoanItem, string)> AddItemTestCase()
        {
            yield return (new LoanItem("Cart", 13, 15.00f), "Cart");
            yield return (new LoanItem("Shoes", 1, 100f), "Shoes");
            yield return (new LoanItem("Chocolate", 10, 1f), "Chocolate");
        }

        private static IEnumerable<(LoanItem, int)> AddItemTestCasePrice()
        {
            yield return (new LoanItem("Cart", 13, 15.00f), 13);
            yield return (new LoanItem("Shoes", 1, 100f), 1);
            yield return (new LoanItem("Chocolate", 10, 1f), 10);
        }

        /*private static IEnumerable<(LoanStack, string)> AddItemTestCase2()
        {
            yield return (new LoanStack("Cart", 13, 15.00f), "Cart");
            yield return (new LoanStack("Chocolate", 10, 1f), "Chocolate");
        }

        private static IEnumerable<(LoanStack, string)> AddItemTestCase3()
        {
            yield return (new LoanStack("Shoes", 1, 100f), "Shoes");
            yield return (new LoanStack("Chocolate", 10, 1f), "Chocolate");
        }

        private static IEnumerable<(LoanStack, string)> AddItemTestCase4()
        {
            yield return (new LoanStack("Cart", 13, 15.00f), "Cart");
            yield return (new LoanStack("Shoes", 1, 100f), "Shoes");
        }

        private static IEnumerable<(LoanStack, string)> AddItemTestCase5()
        {
            yield return (new LoanStack("Cart", 13, 15.00f), "Cart");
        }

        private static IEnumerable<(LoanStack, string)> AddItemTestCase6()
        {
            yield return (new LoanStack("Shoes", 1, 100f), "Shoes");
        }

        private static IEnumerable<(LoanStack, string)> AddItemTestCase7()
        {
            yield return (new LoanStack("Chocolate", 10, 1f), "Chocolate");
        }*/
    }
}
