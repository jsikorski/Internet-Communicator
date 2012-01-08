using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contacts;

namespace Protocol.Statuses
{
    [Serializable]
    public class StatusesRequest : IRequest
    {
        public IEnumerable<Contact> Contacts { get; set; } 
    }
}
