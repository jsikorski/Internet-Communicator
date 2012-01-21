using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Messages;

namespace Client.Features.Messages
{
    public class ConferenceMessageViewModel
    {
        private readonly ConferenceMessage _message;

        public string Date 
        {
            get { return _message.Date.ToShortTimeString(); }
        }

        public string Content
        {
            get { return _message.Content; }
        }

        private readonly string _aggregatedSender;
        public string AggregatedSender
        {
            get { return _aggregatedSender; }
        }

        public ConferenceMessageViewModel(ConferenceMessage message, string aggregatedSender)
        {
            _message = message;
            _aggregatedSender = aggregatedSender;
        }
    }
}
