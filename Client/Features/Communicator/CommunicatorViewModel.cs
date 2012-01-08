using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Services;
using Common.Contacts;
using System.Linq;

namespace Client.Features.Communicator
{
    public class CommunicatorViewModel
    {
        private readonly IServerConnection _serverConnection;
        private readonly IContainer _container;

        public BindableCollection<Contact> Contacts { get; set; }
        public Contact SelectedContact { get; set; }

        public CommunicatorViewModel(
            IServerConnection serverConnection, 
            IContainer container)
        {
            _serverConnection = serverConnection;
            _container = container;

            var contactses = new List<Contact>() { new Contact { Number = 5 }, new Contact { Number = 7 } };
            Contacts = new BindableCollection<Contact>(contactses);
        }

        public void RemoveContact()
        {
            Contacts.Remove(SelectedContact);
        }

        public void ExecuteCommand(string buttonName)
        {
            //_container.Re
        }
    }
}
