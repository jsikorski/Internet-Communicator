using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Services
{
    public class ContactsProvider : IContactsProvider
    {
        private readonly IContactsStorageController _contactsStorageController;

        public ContactsProvider(IContactsStorageController contactsStorageController)
        {
            _contactsStorageController = contactsStorageController;
        }

        public IEnumerable<Contact> GetAll()
        {
            return _contactsStorageController.Load().Select(storedData => new Contact {ContactStoredData = storedData});
        }

        public void Add(Contact contact)
        {
            var contacts = GetAll().ToList();
            contacts.Add(contact);

            _contactsStorageController.Store(contacts.Select(c => c.ContactStoredData));
        }
    }
}
