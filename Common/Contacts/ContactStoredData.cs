using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contacts
{
    [Serializable]
    public class ContactStoredData
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
