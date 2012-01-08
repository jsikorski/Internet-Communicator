using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class AddContactCommand : ICommand<ContactStoredData>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContactsProvider _contactsProvider;

        public AddContactCommand(
            IEventAggregator eventAggregator, 
            IContactsProvider contactsProvider)
        {
            _eventAggregator = eventAggregator;
            _contactsProvider = contactsProvider;
        }

        public void Execute(ContactStoredData contactData)
        {
            var contact = new Contact { ContactStoredData = contactData };
            _contactsProvider.Add(contact);

            _eventAggregator.Publish(new ContactAdded(contact));

        }
    }
}
