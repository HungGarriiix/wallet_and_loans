using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans_test
{
    public class PersonTest
    {
        Person person1;
        Contact contact_inPerson;
        Contact contact_discord;

        [SetUp]
        public void SetUp()
        {
            person1 = new Person(1, "Test");
            contact_inPerson = new Contact(ContactType.IN_PERSON, "In person test", "Just testing");
            contact_discord = new Contact(ContactType.DISCORD, "Discord test", "Just testing");
        }

        [Test]
        public void TestPersonDefaultConstructor()
        {
            int expected_id = 1;
            string expected_name = "Test";

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected_id, person1.ID);
                Assert.AreEqual(expected_name, person1.Name);
            });
        }

        [Test]
        public void TestPersonDataConstructor()
        {
            List<Contact> contacts = new List<Contact>
            {
                contact_discord,
                contact_inPerson
            };
            person1 = new Person(1, "Test", contacts);
            int expected_id = 1;
            string expected_name = "Test";

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected_id, person1.ID);
                Assert.AreEqual(expected_name, person1.Name);
                Assert.AreSame(contacts, person1.Contacts);
                Assert.AreEqual("In person test", person1.GetContactInfo(ContactType.IN_PERSON).ProfileName);
                Assert.AreEqual("Discord test", person1.GetContactInfo(ContactType.DISCORD).ProfileName);
            });
            
        }

        [Test]
        public void TestAddContactInfo()
        {
            person1.AddContactInfo(contact_inPerson);
            person1.AddContactInfo(contact_discord);

            Contact expected_inPerson = contact_inPerson;
            Contact actual_inPerson = person1.GetContactInfo(ContactType.IN_PERSON);
            string expected_inPerson_name = "In person test";
            Contact expected_discord = contact_discord;
            Contact actual_discord = person1.GetContactInfo(ContactType.DISCORD);
            string expected_discord_name = "Discord test";

            Assert.Multiple(() =>
            {
                Assert.AreSame(expected_inPerson, actual_inPerson);
                Assert.AreEqual(expected_inPerson_name, actual_inPerson.ProfileName);
                Assert.AreSame(expected_discord, actual_discord);
                Assert.AreEqual(expected_discord_name, actual_discord.ProfileName);
            });
        }

        [Test]
        public void TestAddExistedContactInfo()
        {
            person1.AddContactInfo(contact_discord);
            Contact another = new Contact(ContactType.DISCORD, "Another discord test", "asd");

            Assert.Throws<Exception>(() => person1.AddContactInfo(another), "The contact exists.");
        }

        [Test]
        public void TestAddTheSameContactObject()
        {
            person1.AddContactInfo(contact_discord);

            Assert.Throws<Exception>(() => person1.AddContactInfo(contact_discord), "The contact exists.");
        }

        [Test]
        public void TestGetContactInfo()
        {
            person1.Contacts.Add(contact_inPerson);
            person1.Contacts.Add(contact_discord);

            Assert.Multiple(() =>
            {
                Assert.AreSame(contact_inPerson, person1.GetContactInfo(ContactType.IN_PERSON));
                Assert.AreSame(contact_discord, person1.GetContactInfo(ContactType.DISCORD));
            });
        }

        [Test]
        public void TestGetUnregisteredContactInfo()
        {
            person1.AddContactInfo(contact_discord);

            Assert.Throws<Exception>(() => person1.GetContactInfo(ContactType.IN_PERSON), "This contact type has not been registered.");
        }

        [Test]
        public void TestUpdateContactInfo()
        {
            person1.AddContactInfo(contact_discord);
            Contact updateContact = new Contact(ContactType.DISCORD, "New discord name changed", "Just another testing");
            person1.UpdateContactInfo(updateContact);
            string expected_name = "New discord name changed";
            string expected_info = "Just another testing";

            Assert.Multiple(() =>
            {
                Assert.AreNotSame(updateContact, person1.GetContactInfo(ContactType.DISCORD));
                Assert.AreEqual(expected_name, person1.GetContactInfo(ContactType.DISCORD).ProfileName);
                Assert.AreEqual(expected_info, person1.GetContactInfo(ContactType.DISCORD).ContactInfo);
            });
        }

        [Test]
        public void TestUpdateContactTypeWithUnregisteredContactType()
        {
            person1.AddContactInfo(contact_discord);
            Contact updateContact = new Contact(ContactType.IN_PERSON, "Supposely update in discord", "Just another testing");
            Assert.Throws<Exception>(() => person1.UpdateContactInfo(updateContact), "This contact type has not been registered.");
        }

        [Test]
        public void TestUpdateContactTypeWithRegisteredContactType()
        {
            person1.AddContactInfo(contact_inPerson);
            person1.AddContactInfo(contact_discord);
            // It is intended to update discord contact instead of in person
            Contact updateContact = new Contact(ContactType.IN_PERSON, "Supposely update in discord", "Just another testing");
            person1.UpdateContactInfo(updateContact);
            string expected_name = "Supposely update in discord";

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(expected_name, person1.GetContactInfo(ContactType.DISCORD).ProfileName);
                Assert.AreEqual(expected_name, person1.GetContactInfo(ContactType.IN_PERSON).ProfileName);
            });
        }

        [Test]
        public void TestDeleteContactInfo()
        {
            person1.AddContactInfo(contact_inPerson);
            person1.DeleteContactInfo(ContactType.IN_PERSON);
            string expected_message = "This contact type has not been registered.";

            Assert.Throws<Exception>(() => person1.GetContactInfo(ContactType.IN_PERSON), expected_message);
        }

        [Test]
        public void TestDeleteNonExistedContact()
        {
            string expected_message = "This contact type has not been registered.";

            Assert.Throws<Exception>(() => person1.DeleteContactInfo(ContactType.IN_PERSON), expected_message);
        }

        [Test]
        public void TestDeleteSameContactTwoTimes()
        {
            person1.AddContactInfo(contact_inPerson);
            person1.DeleteContactInfo(ContactType.IN_PERSON);
            string expected_message = "This contact type has not been registered.";

            Assert.Throws<Exception>(() => person1.DeleteContactInfo(ContactType.IN_PERSON), expected_message);
        }
    }
}
