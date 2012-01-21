using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Caliburn.Micro;
using Client.Context;
using Client.Messages;
using Client.Services;
using Protocol.Messages;
using Timer = System.Timers.Timer;

namespace Client.Commands.Messages
{
    public class StartRequestingForConferencialMessages : ICommand
    {
        private readonly ICurrentContext _currentContext;
        private readonly IServerConnection _serverConnection;
        private readonly IEventAggregator _eventAggregator;

        public StartRequestingForConferencialMessages(
            ICurrentContext currentContext, 
            IServerConnection serverConnection, 
            IEventAggregator eventAggregator)
        {
            _currentContext = currentContext;
            _serverConnection = serverConnection;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            var gettingConferencialMessagesTimer = new Timer(2000) { AutoReset = true };
            _currentContext.GettingConferencialMessagesTimer = gettingConferencialMessagesTimer;

            gettingConferencialMessagesTimer.Elapsed += GetConferencialMessages;
            gettingConferencialMessagesTimer.Start();
        }

        private void GetConferencialMessages(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ConferencialMessagesResponse response;

            try
            {
                response = _serverConnection.SendConferencialMessagesRequest(new ConferenceMessagesRequest());
            }
            catch (Exception)
            {
                Thread.CurrentThread.Abort();
                return;
            }

            if (response.Messages.Any())
            {
                _eventAggregator.Publish(new ConferencialMessagesFounded(response.Messages));
            }
        }
    }
}
