using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Features.Messages
{
    public class ConferencialMessageData
    {
        public IEnumerable<int> ReceiversNumbers { get; private set; }
        public string Content { get; private set; }

        public ConferencialMessageData(IEnumerable<int> receiversNumbers, string content)
        {
            Content = content;
            ReceiversNumbers = receiversNumbers;
        }
    }
}
