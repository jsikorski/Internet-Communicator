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
        private readonly IContainer _container;

        public NewMessagesWindow(
            IWindowManager windowManager,
            ICurrentContext currentContext,
            int connectedContactNumber,
            IContainer container)
        {
            _windowManager = windowManager;
            _currentContext = currentContext;
            _connectedContactNumber = connectedContactNumber;
            _container = container;
        }

        public void Execute()
        {
            var messagesViewModel = _container.Resolve<MessagesViewModel>(
                new UniqueTypeParameter(_connectedContactNumber));

            if (!_currentContext.MessageWindows.ContainsKey(_connectedContactNumber))
            {
                _currentContext.MessageWindows.Add(_connectedContactNumber, messagesViewModel);
                _windowManager.ShowWindow(messagesViewModel);
            }
        }
    }
}
