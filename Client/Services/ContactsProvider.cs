using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;
using Protocol;
using Protocol.Statuses;

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
            var pureContacts = LoadContacts();
            var statusesRequest = new StatusesRequest
                                      {
                                          Contacts = pureContacts.ToList()
                                      };

            var response = _serverConnection.SendStatusesRequest(statusesRequest);

            return response.Contacts;
        }

        public void Add(Contact contact)
        {
            var contacts = GetAll().ToList();
            contacts.Add(contact);

            StoreContacts(contacts);
        }

        public void Remove(Contact contact)
        {
            var contacts = LoadContacts().ToList();
            contacts.Remove(contact);

            StoreContacts(contacts);
        }

        private IEnumerable<Contact> LoadContacts()
        {
            return _contactsStorageController.Load()
                .Select(storedData => new Contact {ContactStoredData = storedData});
        }

        private void StoreContacts(IEnumerable<Contact> contacts)
        {
            _contactsStorageController.Store(contacts.Select(contact => contact.ContactStoredData));
        }
    }
}
