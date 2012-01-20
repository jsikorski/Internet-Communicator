using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.Messages;
using Client.Context;
using Client.Features.Login;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;
using Protocol.Messages;
using Message = Common.Messages.Message;

namespace Client.Features.Messages
{
    public class MessagesViewModel : Screen
    {
        private readonly ICurrentContext _currentContext;
        private readonly Func<MessageRequest, SendMessage> _sendMessageFactory;

        public int ConnectedContactNumber { get; private set; }

        public BindableCollection<MessageViewModel> Messages { get; set; }

        private string _messageContent;
        public string MessageContent
        {
            get { return _messageContent; }
            set
            {
                _messageContent = value;
                NotifyOfPropertyChange(() => CanSendMessage);
                NotifyOfPropertyChange(() => MessageContent);
            }
        }

        public bool CanSendMessage
        {
            get { return !string.IsNullOrEmpty(MessageContent); }
        }

        public MessagesViewModel(
            ICurrentContext currentContext,
            Func<MessageRequest, SendMessage> sendMessageFactory, 
            int connectedContactNumber)
        {
            base.DisplayName = connectedContactNumber.ToString(CultureInfo.InvariantCulture);

            ConnectedContactNumber = connectedContactNumber;
            _currentContext = currentContext;
            _sendMessageFactory = sendMessageFactory;

            Messages = new BindableCollection<MessageViewModel>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(new MessageViewModel(message));
        }

        public void SendMessage()
        {
            int loggedUserNumber = _currentContext.LoggedUserNumber;

            var messageRequest = new MessageRequest(loggedUserNumber, ConnectedContactNumber, MessageContent);
            ICommand command = _sendMessageFactory(messageRequest);
            CommandInvoker.Invoke(command);

            var myMessage = new Message(0, DateTime.Now, MessageContent);
            Messages.Add(new MessageViewModel(myMessage, "Me"));
            MessageContent = string.Empty;
        }

        public void ClearMessages()
        {
            Messages.Clear();
        }

        protected override void OnDeactivate(bool close)
        {
            _currentContext.MessageWindows.Remove(ConnectedContactNumber);
            base.OnDeactivate(close);
        }
    }
}
