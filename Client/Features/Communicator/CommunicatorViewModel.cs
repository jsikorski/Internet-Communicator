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
            IContactsProvider contactsProvider)
        {
            base.DisplayName = "Internet communicator";

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _newContactCommand = newContactCommand;
            _contactsProvider = contactsProvider;

            var contacts = _contactsProvider.GetAll();
            Contacts = new BindableCollection<Contact>(contacts);
        }

        public void NewContact()
        {
            _newContactCommand.Execute();
        }

        public void RemoveContact()
        {
            //_eventAggregator.Publish("Hello");
            //Contacts.Remove(SelectedContact);
        }

        public void Handle(ContactAdded message)
        {
            Contacts.Add(message.Contact);
        }
    }
}
