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
        private readonly Func<int, NewMessagesWindow> _newMessagesWindowFactory;

        public ServiceNewMessages(
            IEnumerable<Message> messages,
            ICurrentContext currentContext,
            Func<int, NewMessagesWindow> newMessagesWindowFactory)
        {
            _messages = messages;
            _currentContext = currentContext;
            _newMessagesWindowFactory = newMessagesWindowFactory;
        }

        public void Execute()
        {
            foreach (var message in _messages)
            {
                int senderNumber = message.SenderNumber;

                if (!_currentContext.MessagesWindows.ContainsKey(senderNumber))
                {
                    ICommand command = _newMessagesWindowFactory(senderNumber);
                    CommandInvoker.Execute(command);
                }

                MessagesViewModel messagesViewModel = _currentContext.MessagesWindows[senderNumber];
                messagesViewModel.AddMessage(message);
            }
        }
    }
}
