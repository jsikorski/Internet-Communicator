using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Context;
using Client.Features.Messages;
using Client.Insrastructure;
using Common.Messages;

namespace Client.Commands.Messages
{
    public class ServiceNewConferencialMessages : ICommand
    {
        private readonly IEnumerable<ConferencialMessage> _conferencialMessages;
        private readonly ICurrentContext _currentContext;
        private readonly Func<IEnumerable<int>, NewConferencialMessagesWindow> _newConferencialMessagesWindowFactory;

        public ServiceNewConferencialMessages(
            IEnumerable<ConferencialMessage> conferencialMessages, 
            ICurrentContext currentContext, 
            Func<IEnumerable<int>, NewConferencialMessagesWindow> newConferencialMessagesWindowFactory)
        {
            _conferencialMessages = conferencialMessages;
            _currentContext = currentContext;
            _newConferencialMessagesWindowFactory = newConferencialMessagesWindowFactory;
        }

        public void Execute()
        {
            foreach (var conferencialMessage in _conferencialMessages)
            {
                IEnumerable<int> collaboratorsNumbers =
                    conferencialMessage.CollaboratorsNumbers.Where(number => number != _currentContext.LoggedUserNumber);

                if (!_currentContext.ConferencialMessagesWindows.ContainsKey(collaboratorsNumbers))
                {
                    var command = _newConferencialMessagesWindowFactory(collaboratorsNumbers);
                    CommandInvoker.Execute(command);
                }

                ConferencialMessagesViewModel messagesViewModel =
                    _currentContext.ConferencialMessagesWindows[collaboratorsNumbers];
                messagesViewModel.AddMessage(conferencialMessage);
            }
        }
    }
}
