using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class RemoveContact : ICommand
    {
        private readonly IContactsProvider _contactsProvider;
        private readonly int _contactIndex;
        private readonly Contact _contact;

        public RemoveContact(
            IContactsProvider contactsProvider,
            int contactIndex)
        {
            _contactsProvider = contactsProvider;
            _contactIndex = contactIndex;
        }

        public void Execute()
        {
            try
            {
                _contactsProvider.Remove(_contactIndex);
            }
            catch (Exception)
            {
                throw new Exception("Cannot remove contact. Please try again later.");
            }
        }
    }
}
