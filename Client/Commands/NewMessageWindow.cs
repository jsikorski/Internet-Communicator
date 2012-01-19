using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Context;
using Client.Features.Messages;

namespace Client.Commands
{
    public class NewMessageWindow : ICommand
    {
        private readonly IWindowManager _windowManager;
        private readonly MessageViewModel _messageViewModel;
        private readonly ICurrentContext _currentContext;

        public NewMessageWindow(
            IWindowManager windowManager, 
            MessageViewModel messageViewModel,
            ICurrentContext currentContext)
        {
            _windowManager = windowManager;
            _messageViewModel = messageViewModel;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            int contactNumber = _messageViewModel.ConnectedContactNumber;

            if (!_currentContext.MessageWindows.ContainsKey(contactNumber))
            {
                _currentContext.MessageWindows.Add(contactNumber, _messageViewModel);
                _windowManager.ShowWindow(_messageViewModel);
            }
        }
    }
}
