using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Common.Contacts;

namespace Client.Features.Communicator
{
    public class ContactViewModel : Screen
    {
        public Contact Contact { get; private set; }

        public string Name
        {
            get { return Contact.ContactStoredData.Name; }
        }

        public int Number
        {
            get { return Contact.ContactStoredData.Number; }
        }

        public bool IsAvailable
        {
            get { return Contact.IsAvailable; }
            set
            {
                Contact.IsAvailable = value;
                NotifyOfPropertyChange(() => IsAvailable);
            }
        }

        public ContactViewModel(Contact contact)
        {
            Contact = contact;
        }
    }
}
