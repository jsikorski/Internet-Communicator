using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Client.Context;
using Client.Features.Login;
using Common.Contacts;

namespace Client.Services
{
    public class ContactsStorageController : IContactsStorageController
    {
        private readonly ICurrentContext _currentContext;
        private const string FilesPathFormat = "{0}.cdat";


        public ContactsStorageController(ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public IEnumerable<ContactStoredData> Load()
        {
            string path = string.Format(FilesPathFormat,
                _currentContext.LoggedUserNumber.ToString(CultureInfo.InvariantCulture));

            if (!File.Exists(path))
            {
                return new List<ContactStoredData>();
            }
            else
            {
                lock (typeof(ContactsStorageController))
                {
                    using (var fileStream = new FileStream(path, FileMode.Open))
                    {
                        var binaryFormatter = new BinaryFormatter();
                        return (IEnumerable<ContactStoredData>)binaryFormatter.Deserialize(fileStream);
                    }
                }
            }
        }

        public void Store(IEnumerable<ContactStoredData> contactsData)
        {
            string path = string.Format(FilesPathFormat,
                _currentContext.LoggedUserNumber.ToString(CultureInfo.InvariantCulture));

            lock (typeof(ContactsStorageController))
            {
                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, contactsData.ToList());
                }
            }
        }
    }
}
