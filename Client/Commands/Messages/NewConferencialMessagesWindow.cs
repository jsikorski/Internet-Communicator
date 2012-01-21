using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Context;
using Client.Features.Messages;

namespace Client.Commands.Messages
{
    public class NewConferencialMessagesWindow : ICommand
    {
        private readonly IEnumerable<int> _connectedNumbers;
        private readonly IWindowManager _windowManager;
        private readonly ICurrentContext _currentContext;
        private readonly Func<IEnumerable<int>, ConferencialMessagesViewModel> _conferencialMessagesViewModelFactory;

        public NewConferencialMessagesWindow(
            IEnumerable<int> connectedNumbers,
            IWindowManager windowManager,
            ICurrentContext currentContext,
            Func<IEnumerable<int>, ConferencialMessagesViewModel> conferencialMessagesViewModelFactory)
        {
            _connectedNumbers = connectedNumbers;
            _windowManager = windowManager;
            _currentContext = currentContext;
            _conferencialMessagesViewModelFactory = conferencialMessagesViewModelFactory;
        }

        public void Execute()
        {
            var viewModel = _conferencialMessagesViewModelFactory(_connectedNumbers);

            if (!_currentContext.ConferencialMessagesWindows.ContainsKey(_connectedNumbers))
            {
                _windowManager.ShowWindow(viewModel);
                Caliburn.Micro.Execute.OnUIThread(
                    () => _currentContext.ConferencialMessagesWindows.Add(_connectedNumbers, viewModel));
            }
        }
    }
}
