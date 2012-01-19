using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Caliburn.Micro;
using Client.Commands;
using Client.Context;
using Client.Features.Login;
using Client.Insrastructure;
using Client.Messages;
using Client.Utils;
using Protocol.Messages;
using Message = Common.Messages.Message;

namespace Client.Features.Messages
{
    public class MessageViewModel : Screen
    {
        private readonly ICurrentContext _currentContext;
        private readonly LoggedUser _loggedUser;
        private readonly IContainer _container;

        public BindableCollection<Message> Messages { get; set; }
        public int ConnectedContactNumber { get; private set; }

        public string MessageContent { get; set; }

        public MessageViewModel(
            ICurrentContext currentContext,
            int connectedContactNumber,
            LoggedUser loggedUser,
            IContainer container)
        {
            base.DisplayName = connectedContactNumber.ToString(CultureInfo.InvariantCulture);

            ConnectedContactNumber = connectedContactNumber;
            _currentContext = currentContext;
            _loggedUser = loggedUser;
            _container = container;
            Messages = new BindableCollection<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }

        public void SendMessage()
        {
            var messageRequest = new MessageRequest(
                _loggedUser.Number, new List<int> { ConnectedContactNumber }, MessageContent);
            ICommand command = _container.Resolve<SendMessage>(new UniqueTypeParameter(messageRequest));
            CommandInvoker.Invoke(command);
        }

        protected override void OnDeactivate(bool close)
        {
            _currentContext.MessageWindows.Remove(ConnectedContactNumber);
            base.OnDeactivate(close);
        }
    }
}
