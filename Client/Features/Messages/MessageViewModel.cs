using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Context;
using Client.Messages;
using Message = Common.Messages.Message;

namespace Client.Features.Messages
{
    public class MessageViewModel : Screen
    {
        private readonly ICurrentContext _currentContext;

        public BindableCollection<Message> Messages { get; set; }
        public int ConnectedContactNumber { get; private set; }

        public MessageViewModel(
            ICurrentContext currentContext,
            int connectedContactNumber)
        {
            base.DisplayName = connectedContactNumber.ToString(CultureInfo.InvariantCulture);

            ConnectedContactNumber = connectedContactNumber;
            _currentContext = currentContext;
            Messages = new BindableCollection<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);   
        }

        protected override void OnDeactivate(bool close)
        {
            _currentContext.MessageWindows.Remove(ConnectedContactNumber);
            base.OnDeactivate(close);
        }
    }
}
