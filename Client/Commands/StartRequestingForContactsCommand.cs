using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Caliburn.Micro;
using Client.Context;
using Client.Messages;
using Common.Contacts;
using Timer = System.Timers.Timer;

namespace Client.Commands
{
    public class StartRequestingForContactsCommand : ICommand
    {
        private readonly GetContactsCommand _getContactsCommand;
        private readonly ICurrentContext _currentContext;
        private readonly IEventAggregator _eventAggregator;

        public StartRequestingForContactsCommand(
            GetContactsCommand getContactsCommand,
            ICurrentContext currentContext,
            IEventAggregator eventAggregator)
        {
            _getContactsCommand = getContactsCommand;
            _currentContext = currentContext;
            _eventAggregator = eventAggregator;
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
            IEnumerable<Contact> contacts = _getContactsCommand.Execute();
            _eventAggregator.Publish(new ContactsDataReceived(contacts));
        }
    }
}
