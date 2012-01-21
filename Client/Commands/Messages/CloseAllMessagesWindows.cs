using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;

namespace Client.Commands.Messages
{
    public class CloseAllMessagesWindows : ICommand
    {
        private readonly ICurrentContext _currentContext;

        public CloseAllMessagesWindows(
            ICurrentContext currentContext)
        {
            _currentContext = currentContext;
        }

        public void Execute()
        {
            foreach (var messagesViewModel in _currentContext.MessagesWindows)
            {
                messagesViewModel.Value.TryClose();
            }
        }
    }
}
