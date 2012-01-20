using System;
using Autofac;
using Caliburn.Micro;
using Client.Context;
using Client.Features.Messages;
using Client.Utils;

namespace Client.Commands.Messages
{
    public class NewMessagesWindow : ICommand
    {
        private readonly IWindowManager _windowManager;
        private readonly ICurrentContext _currentContext;
        private readonly int _connectedContactNumber;
        private readonly Func<int, MessagesViewModel> _messageViewModelFactory;

        public NewMessagesWindow(
            IWindowManager windowManager,
            ICurrentContext currentContext,
            Func<int, MessagesViewModel> messagesViewModelFactory,
            int connectedContactNumber)
        {
            _windowManager = windowManager;
            _currentContext = currentContext;
            _connectedContactNumber = connectedContactNumber;
            _messageViewModelFactory = messagesViewModelFactory;
        }

        public void Execute()
        {
            var messagesViewModel = _messageViewModelFactory(_connectedContactNumber);

            if (!_currentContext.MessageWindows.ContainsKey(_connectedContactNumber))
            {
                _currentContext.MessageWindows.Add(_connectedContactNumber, messagesViewModel);
                Caliburn.Micro.Execute.OnUIThread(() => _windowManager.ShowWindow(messagesViewModel));
            }
        }
    }
}
