using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Client.Features.Messages
{
    public class ConferencialMessageViewModel
    {
        private readonly ConferencialMessage _message;

        public string Date 
        {
            get { return _message.Date.ToShortTimeString(); }
        }

        public string Content
        {
            get { return _message.Content; }
        }

        private readonly string _sender;
        public string Sender
        {
            get { return _sender; }
        }

        public ConferencialMessageViewModel(ConferencialMessage message, string sender)
        {
            _message = message;
            _sender = sender;
        }
    }
}
