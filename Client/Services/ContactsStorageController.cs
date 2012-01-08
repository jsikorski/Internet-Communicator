using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Client.Features.Login;
using Common.Contacts;

namespace Client.Services
{
    public class ContactsStorageController : IContactsStorageController
    {
        private const string FilesPathFormat = "{0}.cdat";

        private readonly LoggedUser _loggedUser;

        public ContactsStorageController(LoggedUser loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public IEnumerable<ContactStoredData> Load()
        {
            string path = string.Format(FilesPathFormat, _loggedUser.Number.ToString());

            if (!File.Exists(path))
            {
                return new List<ContactStoredData>();
            }
            else
            {
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    var binaryFormatter = new BinaryFormatter();
                    return (IEnumerable<ContactStoredData>)binaryFormatter.Deserialize(fileStream);
                }
            }
        }

        public void Store(IEnumerable<ContactStoredData> contactsData)
        {
            string path = string.Format(FilesPathFormat, _loggedUser.Number.ToString());

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, contactsData.ToList());
            }
        }
    }
}
