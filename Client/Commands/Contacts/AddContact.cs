﻿using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Contacts;
using Protocol.Statuses;

namespace Client.Commands.Contacts
{
    public class AddContact : ICommand
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContactsProvider _contactsProvider;
        private readonly IServerConnection _serverConnection;
        private readonly ContactStoredData _contactData;

        public AddContact(
            IEventAggregator eventAggregator,
            IContactsProvider contactsProvider,
            IServerConnection serverConnection,
            ContactStoredData contactData)
        {
            _eventAggregator = eventAggregator;
            _contactsProvider = contactsProvider;
            _serverConnection = serverConnection;
            _contactData = contactData;
        }

        public void Execute()
        {
            var contact = new Contact { ContactStoredData = _contactData };

            _contactsProvider.Add(contact);
            Contact responseContact = _serverConnection.SendStatusesRequest(
                new StatusesRequest { Contacts = new List<Contact> { contact } }).Contacts.First();

            _eventAggregator.Publish(new ContactAdded(responseContact));
        }
    }
}
