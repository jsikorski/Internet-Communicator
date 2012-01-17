using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Messages
{
    public class ContactsLoaded
    {
        public IEnumerable<Contact> Contacts { get; private set; }
 
        public ContactsLoaded(IEnumerable<Contact> contacts)
        {
            Contacts = contacts;
        }
    }
}
