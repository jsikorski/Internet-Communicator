using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Client.Context;
using Client.Features.Messages;
using Client.Insrastructure;
using Client.Utils;
using Common.Messages;

namespace Client.Commands
{
    public class ServiceNewMessages : ICommand
    {
        private readonly IEnumerable<Message> _messages;
        private readonly ICurrentContext _currentContext;
        private readonly IContainer _container;

        public ServiceNewMessages(
            IEnumerable<Message> messages,
            ICurrentContext currentContext,
            IContainer container)
        {
            _messages = messages;
            _currentContext = currentContext;
            _container = container;
        }

        public void Execute()
        {
            foreach (var message in _messages)
            {
                int senderNumber = message.SenderNumber;

                MessageViewModel messageViewModel;
                if (_currentContext.MessageWindows.ContainsKey(senderNumber))
                {
                    messageViewModel = _currentContext.MessageWindows[senderNumber];
                }
                else
                {
                    messageViewModel = _container.Resolve<MessageViewModel>(new UniqueTypeParameter(senderNumber));
                    ICommand command = _container.Resolve<NewMessageWindow>(new UniqueTypeParameter(messageViewModel));
                    CommandInvoker.Execute(command);
                }

                messageViewModel.AddMessage(message);
            }
        }
    }
}
