using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Messages
{
    public class ContactsDataReceived
    {
        public IEnumerable<Contact> Contacts { get; private set; }

        public ContactsDataReceived(IEnumerable<Contact> contacts)
        {
            Contacts = contacts;
        }
    }
}
