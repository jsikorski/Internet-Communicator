using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Client.Commands.Messages;
using Client.Features.Messages;

namespace Client.Context
{
    public class CurrentContext : ICurrentContext
    {
        public Timer GettingContactsTimer { get; set; }
        public Timer GettingMessagesTimer { get; set; }
        public Timer GettingFilesTimer { get; set; }

        private readonly Dictionary<int, MessagesViewModel> _messagesWindows;
        public Dictionary<int, MessagesViewModel> MessagesWindows
        {
            get { return _messagesWindows; }
        }

        private readonly Dictionary<IEnumerable<int>, ConferencialMessagesViewModel> _conferencialMessagesWindows;
        public Dictionary<IEnumerable<int>, ConferencialMessagesViewModel> ConferencialMessagesWindows
        {
            get { return _conferencialMessagesWindows; }
        }

        public int LoggedUserNumber { get; set; }

        public CurrentContext()
        {
            _messagesWindows = new Dictionary<int, MessagesViewModel>();
            _conferencialMessagesWindows = new Dictionary<IEnumerable<int>, ConferencialMessagesViewModel>(
                new ConferencialWindowsEqualityComparer());
        }
    }
}
