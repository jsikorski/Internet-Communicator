using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Contacts;
using Client.Messages;
using Client.Services;
using Client.Utils;
using Common.Contacts;
using System.Linq;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel : Screen, IHandle<ContactAdded>, IHandle<ContactsDataReceived>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly NewContact _newContact;
        private readonly RemoveContact _removeContact;
        private readonly GetContacts _getContacts;
        private readonly StartRequestingForContacts _startRequestingForContacts;
        private readonly StopRequestingForContacts _stopRequestingForContacts;

        public BindableCollection<ContactViewModel> Contacts { get; set; }
        private ContactViewModel _selectedContact;
        public ContactViewModel SelectedContact
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
            NewContact newContact,
            RemoveContact removeContact,
            GetContacts getContacts,
            StartRequestingForContacts startRequestingForContacts,
            StopRequestingForContacts stopRequestingForContacts)
        {
            base.DisplayName = "Internet communicator";

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _newContact = newContact;
            _removeContact = removeContact;
            _getContacts = getContacts;
            _startRequestingForContacts = startRequestingForContacts;
            _stopRequestingForContacts = stopRequestingForContacts;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            IEnumerable<Contact> contacts = _getContacts.Execute();
            Contacts = new BindableCollection<ContactViewModel>(
                contacts.Select(contact => new ContactViewModel(contact)));
            
            _startRequestingForContacts.Execute();
        }

        protected override void OnDeactivate(bool close)
        {
            _stopRequestingForContacts.Execute();
            base.OnDeactivate(close);
        }

        public void NewContact()
        {
            _newContact.Execute();
        }

        public void RemoveContact()
        {
            _removeContact.Execute(SelectedContact.Contact);
            Contacts.Remove(SelectedContact);
        }

        public void Handle(ContactAdded message)
        {
            Contacts.Add(new ContactViewModel(message.Contact));
        }

        public void Handle(ContactsDataReceived message)
        {
            foreach (var contactViewModel in Contacts)
            {
                contactViewModel.IsAvailable =
                    message.Contacts.First(c => c.ContactStoredData.Number == contactViewModel.Number).IsAvailable;
            }
        }
    }
}
