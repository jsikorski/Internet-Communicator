using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Client.Services;
using Common.Contacts;

namespace Client.Commands
{
    public class GetContacts
    {
        private readonly IContactsProvider _contactsProvider;

        public GetContacts(
            IContactsProvider contactsProvider)
        {
            _contactsProvider = contactsProvider;
        }

        public IEnumerable<Contact> Execute()
        {
            IEnumerable<Contact> contacts;
            try
            {
                contacts = _contactsProvider.GetAll();                
            }
            catch (Exception)
            {
                throw new Exception("Cannot get contacts from sever.");
            }

            return contacts;
        }
    }
}
