using System;
using Client.Services;

namespace Client.Commands.Contacts
{
    public class RemoveContact : ICommand
    {
        private readonly IContactsProvider _contactsProvider;
        private readonly int _contactIndex;

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
