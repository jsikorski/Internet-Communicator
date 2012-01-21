using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Messages
{
    [Serializable]
    public class ConferenceMessageRequest : IRequest
    {
        public int Sender { get; private set; }
        public IEnumerable<int> ReciversNumbers { get; private set; }
        public string Text { get; private set; }

        public ConferenceMessageRequest(int sender, string text, IEnumerable<int> reciversNumbers)
        {
            ReciversNumbers = reciversNumbers;
            Text = text;
            Sender = sender;
        }
    }
}
