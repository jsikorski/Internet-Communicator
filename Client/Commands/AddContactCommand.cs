using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Contacts;
using Protocol.Statuses;

namespace Client.Commands
{
    public class AddContactCommand : ICommand<ContactStoredData>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContactsProvider _contactsProvider;
        private readonly IServerConnection _serverConnection;

        public AddContactCommand(
            IEventAggregator eventAggregator,
            IContactsProvider contactsProvider,
            IServerConnection serverConnection)
        {
            _eventAggregator = eventAggregator;
            _contactsProvider = contactsProvider;
            _serverConnection = serverConnection;
        }

        public void Execute(ContactStoredData contactData)
        {
            var contact = new Contact { ContactStoredData = contactData };

            try
            {
                _contactsProvider.Add(contact);
                _serverConnection.SendStatusesRequest(new StatusesRequest { Contacts = new List<Contact> { contact } });
            }
            catch (Exception)
            {
                throw new Exception("Cannot add new contact. Please try again later.");
            }

            _eventAggregator.Publish(new ContactAdded(contact));
        }
    }
}
