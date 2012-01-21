using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;

namespace Client.Commands.Messages
{
    public class StopRequestingForConferencialMessages : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public StopRequestingForConferencialMessages(
            ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            _currentContext.GettingConferencialMessagesTimer.Stop();
        }
    }
}
