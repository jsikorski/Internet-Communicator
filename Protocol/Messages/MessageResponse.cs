using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Messages
{
    [Serializable]
    public class MessageResponse : IResponse
    {
        public int Sender { get; set; }
        public IEnumerable<int> ReceiversNumbers { get; set; }
        public string Text { get; set; }
    }
}
