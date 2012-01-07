using System.Collections.Generic;
using Caliburn.Micro;
using Client.Services;
using Common.Contacts;
using System.Linq;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel
    {
        public BindableCollection<Contact> Contacts { get; set; }
        public Contact SelectedContact { get; set; }

        public IList<Contact> Contactses { get; set; }

        private readonly IServerConnection _serverConnection;

        public CommunicatorViewModel(
            IServerConnection serverConnection)
        {
            _serverConnection = serverConnection;

            Contactses = new List<Contact>() { new Contact { Number = 5 }, new Contact { Number = 7 } };
            Contacts = new BindableCollection<Contact>(Contactses);
        }

        public void RemoveContact()
        {
            Contacts.Remove(SelectedContact);
            //Contacts.Remove(SelectedContact);
        }
    }
}
