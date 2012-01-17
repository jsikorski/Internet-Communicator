using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class LoadContacts : ICommand
    {
        private readonly IContactsProvider _contactsProvider;
        private readonly IEventAggregator _eventAggregator;

        public LoadContacts(
            IContactsProvider contactsProvider,
            IEventAggregator eventAggregator)
        {
            _contactsProvider = contactsProvider;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            IEnumerable<Contact> contacts;
            try
            {
                contacts = _contactsProvider.GetAll();                
            }
            catch (Exception)
            {
                throw new Exception("Cannot load contacts sever.");
            }

            _eventAggregator.Publish(new ContactsLoaded(contacts));           
        }
    }
}
