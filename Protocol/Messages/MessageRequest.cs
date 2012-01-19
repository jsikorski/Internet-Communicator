using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Messages
{
    [Serializable]
    public class MessageRequest : IRequest
    {
        public int Sender { get; private set; }
        public IEnumerable<int> ReceiversNumbers { get; private set; }
        public string Text { get; private set; }

        public MessageRequest(int sender, 
            IEnumerable<int> receiversNumbers, string text)
        {
            Text = text;
            ReceiversNumbers = receiversNumbers;
            Sender = sender;
        }
    }
}
