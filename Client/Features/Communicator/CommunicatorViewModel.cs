using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Features.Contacts;
using Client.Insrastructure;
using Client.Messages;
using Client.Services;
using Client.Utils;
using Common.Contacts;
using System.Linq;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel : Screen, IHandle<ContactAdded>, IHandle<ContactsDataReceived>, IHandle<ContactsLoaded>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainer _container;

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
        public int SelectedContactIndex { get; set; }

        public bool CanRemoveContact
        {
            get { return SelectedContact != null; }
        }

        public CommunicatorViewModel(
            IEventAggregator eventAggregator,
            IContainer container)
        {
            base.DisplayName = "Internet communicator";

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _container = container;
            Contacts = new BindableCollection<ContactViewModel>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ExecutePureCommand<LoadContacts>();
            ExecutePureCommand<StartRequestingForContacts>();
        }

        protected override void OnDeactivate(bool close)
        {
            ExecutePureCommand<StopRequestingForContacts>();
            base.OnDeactivate(close);
        }

        public void NewContact()
        {
            ExecutePureCommand<NewContact>();
        }

        public IEnumerable<IResult> RemoveContact()
        {
            ICommand removeContact = _container.Resolve<RemoveContact>(
                new UniqueTypeParameter(SelectedContactIndex));
            yield return new CommandResult(removeContact);
            Contacts.Remove(SelectedContact);
        }

        public void Handle(ContactAdded message)
        {
            Contacts.Add(new ContactViewModel(message.Contact));
        }

        public void Handle(ContactsLoaded message)
        {
            Contacts.AddRange(message.Contacts.Select(contact => new ContactViewModel(contact)));
        }

        public void Handle(ContactsDataReceived message)
        {
            foreach (var contactViewModel in Contacts)
            {
                contactViewModel.IsAvailable =
                    message.Contacts.First(c => c.ContactStoredData.Number == contactViewModel.Number).IsAvailable;
            }
        }

        private void ExecutePureCommand<T>() where T : ICommand
        {
            ICommand command = _container.Resolve<T>();
            CommandInvoker.Execute(command);
        }
    }
}
