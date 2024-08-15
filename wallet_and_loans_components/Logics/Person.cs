using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallet_and_loans.Logics
{
    public class Person
    {
        public Person(int id, string name)
        {
            ID = id;
            Name = name;
            Contacts = new List<Contact>();
        }

        // get from db
        public Person(int id, string name, List<Contact> contacts) : this(id, name) 
        {
            Contacts = contacts;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; }

        public void AddContactInfo(Contact contact) 
        {
            foreach(Contact main_cont in Contacts) 
            {
                if (main_cont.TypeOfContact == contact.TypeOfContact) throw new Exception("The contact exists.");
            }
            Contacts.Add(contact);
        }

        public Contact GetContactInfo(ContactType type)
        {
            foreach (Contact contact in Contacts)
            {
                if (contact.TypeOfContact == type) return contact;
            }

            throw new Exception("This contact type has not been registered.");
        }

        public void UpdateContactInfo(Contact updated_contact)
        {
            Contact contact = GetContactInfo(updated_contact.TypeOfContact);
            contact.ContactInfo = updated_contact.ContactInfo;
            contact.ProfileName = updated_contact.ProfileName; 
        }

        public void DeleteContactInfo(ContactType type)
        {
            Contacts.Remove(GetContactInfo(type));
        }
    }
}
