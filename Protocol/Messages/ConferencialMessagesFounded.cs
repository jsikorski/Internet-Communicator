using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Protocol.Messages
{
    public class ConferencialMessagesFounded
    {
        public IEnumerable<ConferencialMessage> ConferencialsMessages { get; private set; }

        public ConferencialMessagesFounded(IEnumerable<ConferencialMessage> conferencialsMessages)
        {
            ConferencialsMessages = conferencialsMessages;
        }
    }
}
