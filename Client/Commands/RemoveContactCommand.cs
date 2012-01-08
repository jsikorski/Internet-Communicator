using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class RemoveContactCommand : ICommand<Contact>
    {
        private readonly IContactsProvider _contactsProvider;

        public RemoveContactCommand(IContactsProvider contactsProvider)
        {
            _contactsProvider = contactsProvider;
        }

        public void Execute(Contact contact)
        {
            _contactsProvider.Remove(contact);
        }
    }
}
