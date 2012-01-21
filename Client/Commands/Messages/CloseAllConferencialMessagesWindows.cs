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
            _currentContext.ConferencialMessagesWindows.ToList()
                .ForEach(keyValuePair => keyValuePair.Value.TryClose());
        }
    }
}
