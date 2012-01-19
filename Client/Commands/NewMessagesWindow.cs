using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Context;
using Client.Features.Messages;

namespace Client.Commands
{
    public class NewMessagesWindow : ICommand
    {
        private readonly IWindowManager _windowManager;
        private readonly MessagesViewModel _messagesViewModel;
        private readonly ICurrentContext _currentContext;

        public NewMessagesWindow(
            IWindowManager windowManager, 
            MessagesViewModel messagesViewModel,
            ICurrentContext currentContext)
        {
            _windowManager = windowManager;
            _messagesViewModel = messagesViewModel;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            int contactNumber = _messagesViewModel.ConnectedContactNumber;

            if (!_currentContext.MessageWindows.ContainsKey(contactNumber))
            {
                _currentContext.MessageWindows.Add(contactNumber, _messagesViewModel);
                _windowManager.ShowWindow(_messagesViewModel);
            }
        }
    }
}
