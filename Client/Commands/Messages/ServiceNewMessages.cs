using System;
using System.Collections.Generic;
using Autofac;
using Client.Context;
using Client.Features.Messages;
using Client.Insrastructure;
using Client.Utils;
using Common.Messages;

namespace Client.Commands.Messages
{
    public class ServiceNewMessages : ICommand
    {
        private readonly IEnumerable<Message> _messages;
        private readonly ICurrentContext _currentContext;
        private readonly Func<int, NewMessagesWindow> _messagesWindowFactory;

        public ServiceNewMessages(
            IEnumerable<Message> messages,
            ICurrentContext currentContext,
            Func<int, NewMessagesWindow> messagesWindowFactory)
        {
            _messages = messages;
            _currentContext = currentContext;
            _messagesWindowFactory = messagesWindowFactory;
        }

        public void Execute()
        {
            foreach (var message in _messages)
            {
                int senderNumber = message.SenderNumber;

                if (!_currentContext.MessageWindows.ContainsKey(senderNumber))
                {
                    ICommand command = _messagesWindowFactory(senderNumber);
                    CommandInvoker.Execute(command);
                }

                MessagesViewModel messagesViewModel = _currentContext.MessageWindows[senderNumber];
                messagesViewModel.AddMessage(message);
            }
        }
    }
}
