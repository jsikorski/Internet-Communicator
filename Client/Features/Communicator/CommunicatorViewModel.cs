using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Contacts;
using Client.Messages;
using Client.Services;
using Common.Contacts;
using System.Linq;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel : Screen, IHandle<ContactAdded>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly NewContactCommand _newContactCommand;
        private readonly RemoveContactCommand _removeContactCommand;
        private readonly IContactsProvider _contactsProvider;

        public BindableCollection<Contact> Contacts { get; set; }
        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                NotifyOfPropertyChange(() => CanRemoveContact);
            }
        }

        public bool CanRemoveContact
        {
            get { return SelectedContact != null; }
        }

        public CommunicatorViewModel(
            IEventAggregator eventAggregator, 
            NewContactCommand newContactCommand,
            RemoveContactCommand removeContactCommand,
            IContactsProvider contactsProvider)
        {
            base.DisplayName = "Internet communicator";

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _newContactCommand = newContactCommand;
            _removeContactCommand = removeContactCommand;
            _contactsProvider = contactsProvider;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var contacts = _contactsProvider.GetAll();
            Contacts = new BindableCollection<Contact>(contacts);
        }

        public void NewContact()
        {
            _newContactCommand.Execute();
        }

        public void RemoveContact()
        {
            _removeContactCommand.Execute(SelectedContact);
            Contacts.Remove(SelectedContact);
        }

        public void Handle(ContactAdded message)
        {
            Contacts.Add(message.Contact);
        }
    }
}
