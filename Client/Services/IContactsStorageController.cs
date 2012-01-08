using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Client.Services
{
    public interface IContactsStorageController
    {
        IEnumerable<ContactStoredData> Load();
        void Store(IEnumerable<ContactStoredData> contactsData);
    }
}
