using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;

namespace Client.Commands.Messages
{
    public class CloseAllConferencialMessagesWindows : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public CloseAllConferencialMessagesWindows(
            ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            foreach (var conferencialMessagesViewModel in _currentContext.ConferencialMessagesWindows)
            {
                conferencialMessagesViewModel.Value.TryClose();
            }
        }
    }
}
