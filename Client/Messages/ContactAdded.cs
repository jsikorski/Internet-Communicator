using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Messages
{
    public class ContactAdded
    {
        public Contact Contact { get; private set; }

        public ContactAdded(Contact contact)
        {
            Contact = contact;
        }
    }
}
