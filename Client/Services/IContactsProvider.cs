using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Services
{
    public interface IContactsProvider
    {
        IEnumerable<Contact> GetAll();
        void Add(Contact contact);
    }
}
