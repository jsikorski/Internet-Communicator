using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Commands;
using Client.Commands.Messages;
using Client.Context;
using Client.Insrastructure;
using Client.Services;
using Common.Messages;
using Protocol.Messages;
using Message = Common.Messages.Message;

namespace Client.Features.Messages
{
    public class ConferencialMessagesViewModel : Screen
    {
        private readonly ICurrentContext _currentContext;
        private readonly Func<ConferencialMessageData, SendConferencialMessage> _sendConferenceMessageFactory;
        private readonly INumbersToNamesConverter _numbersToNamesConverter;

        public IEnumerable<int> ConnectedContactsNumber { get; private set; }

        public BindableCollection<ConferenceMessageViewModel> Messages { get; set; }

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

        public ConferencialMessagesViewModel(
            ICurrentContext currentContext,
            Func<ConferencialMessageData, SendConferencialMessage> sendConferenceMessageFactory, 
            INumbersToNamesConverter numbersToNamesConverter,
            IEnumerable<int> connectedContactNumbers)
        {
            base.DisplayName = numbersToNamesConverter.ConvertGroup(connectedContactNumbers);

            ConnectedContactsNumber = connectedContactNumbers;
            _currentContext = currentContext;
            _sendConferenceMessageFactory = sendConferenceMessageFactory;
            _numbersToNamesConverter = numbersToNamesConverter;

            Messages = new BindableCollection<ConferenceMessageViewModel>();
        }

        public void AddMessage(ConferencialMessage message)
        {
            Messages.Add(new ConferenceMessageViewModel(message, 
                _numbersToNamesConverter.Convert(message.SenderNumber)));
        }

        public void SendMessage()
        {
            var messageData = new ConferencialMessageData(ConnectedContactsNumber.ToList(), MessageContent);
            ICommand command = _sendConferenceMessageFactory(messageData);
            CommandInvoker.Invoke(command);

            var myMessage = new ConferencialMessage(0, DateTime.Now, MessageContent, null);
            Messages.Add(new ConferenceMessageViewModel(myMessage, "Me"));
            MessageContent = string.Empty;
        }

        public void ClearMessages()
        {
            Messages.Clear();
        }

        protected override void OnDeactivate(bool close)
        {
            _currentContext.ConferencialMessagesWindows.Remove(ConnectedContactsNumber);
            base.OnDeactivate(close);
        }
    }
}
