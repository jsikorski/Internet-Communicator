using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;
using Common.Contacts;

namespace Client.Features.Contacts
{
    public class NewContactViewModel : Screen, IBusyScope, IHandle<ContactAdded>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainer _container;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public bool CanAdd
        {
            get { return !string.IsNullOrEmpty(Name) && 
                !string.IsNullOrEmpty(Number); }
        }

        public NewContactViewModel(
            IEventAggregator eventAggregator,
            IContainer container)
        {
            base.DisplayName = "Add new contact";
            _eventAggregator = eventAggregator;
            _container = container;
        }

        public void Add()
        {
            _eventAggregator.Subscribe(this);
            var contactData = new ContactStoredData
                                  {
                                      Name = Name,
                                      Number = Int32.Parse(Number)
                                  };
            ICommand addContact = _container.Resolve<AddContact>(new UniqueTypeParameter(contactData));
            CommandInvoker.InvokeBusy(addContact, this);
        }

        public void Handle(ContactAdded message)
        {
            _eventAggregator.Unsubscribe(this);
            TryClose();
        }
    }
}
