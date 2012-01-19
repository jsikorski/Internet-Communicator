using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Client.Features.Messages
{
    public class MessageViewModel
    {
        private readonly Message _message;
        private readonly string _sender;

        public string Date 
        {
            get { return _message.Date.ToShortTimeString(); }
        }

        public string Sender
        {
            get
            {
                if (!string.IsNullOrEmpty(_sender))
                {
                    return _sender;
                }

                return _message.SenderNumber.ToString(CultureInfo.InvariantCulture);
            }
        }
        
        public string Content
        {
            get { return _message.Content; }
        }

        public MessageViewModel(Message message, string sender = null)
        {
            _message = message;
            _sender = sender;
        }
    }
}
