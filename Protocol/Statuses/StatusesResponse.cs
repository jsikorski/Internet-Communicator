using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Protocol.Statuses
{
    [Serializable]
    public class StatusesResponse : IResponse
    {
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
