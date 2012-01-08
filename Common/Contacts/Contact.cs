using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contacts
{
    [Serializable]
    public class Contact
    {
        public ContactStoredData ContactStoredData { get; set; }
        public bool IsAvailable { get; set; }
    }
}
