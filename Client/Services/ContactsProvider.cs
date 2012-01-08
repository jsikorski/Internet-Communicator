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
        private readonly IServerConnection _serverConnection;

        public ContactsProvider(IContactsStorageController contactsStorageController, 
            IServerConnection serverConnection)
        {
            _contactsStorageController = contactsStorageController;
            _serverConnection = serverConnection;
        }

        public IEnumerable<Contact> GetAll()
        {
            var pureContacts = _contactsStorageController.Load().Select(storedData => new Contact {ContactStoredData = storedData});

            return null;
        }

        public void Add(Contact contact)
        {
            var contacts = GetAll().ToList();
            contacts.Add(contact);

            _contactsStorageController.Store(contacts.Select(c => c.ContactStoredData));
        }
    }
}
