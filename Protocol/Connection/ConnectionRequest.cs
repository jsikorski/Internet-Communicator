using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Connection
{
    [Serializable]
    public class ConnectionRequest : IRequest
    {
        public string ClientListeningAddress { get; set; }
        public int ClientListenerPortNumber { get; set; }
    }
}
