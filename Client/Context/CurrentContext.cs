using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Client.Features.Messages;

namespace Client.Context
{
    public class CurrentContext : ICurrentContext
    {
        public Timer GettingContactsTimer { get; set; }
        public Timer GettingMessagesTimer { get; set; }

        private readonly Dictionary<int, MessageViewModel> _messageWindows;
        public Dictionary<int, MessageViewModel> MessageWindows
        {
            get { return _messageWindows; }
        }

        public CurrentContext()
        {
            _messageWindows = new Dictionary<int, MessageViewModel>();            
        }
    }
}
