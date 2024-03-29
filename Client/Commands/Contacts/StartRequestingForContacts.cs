﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Caliburn.Micro;
using Client.Context;
using Client.Messages;
using Client.Services;
using Common.Contacts;
using Timer = System.Timers.Timer;

namespace Client.Commands.Contacts
{
    public class StartRequestingForContacts : ICommand
    {
        private readonly ICurrentContext _currentContext;
        private readonly IEventAggregator _eventAggregator;
        private readonly IContactsProvider _contactsProvider;

        public StartRequestingForContacts(
            ICurrentContext currentContext,
            IEventAggregator eventAggregator,
            IContactsProvider contactsProvider)
        {
            _currentContext = currentContext;
            _eventAggregator = eventAggregator;
            _contactsProvider = contactsProvider;
        }

        public void Execute()
        {
            var gettingContactsTimer = new Timer(2000) {AutoReset = true};
            _currentContext.GettingContactsTimer = gettingContactsTimer;
            
            gettingContactsTimer.Elapsed += UpdateContacts;
            gettingContactsTimer.Start();
        }

        private void UpdateContacts(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            IEnumerable<Contact> contacts;
            try
            {
                contacts  = _contactsProvider.GetAll();;
            }
            catch (Exception)
            {
                Thread.CurrentThread.Abort();
                return;
            }

            _eventAggregator.Publish(new ContactsDataReceived(contacts, false));
        }
    }
}
