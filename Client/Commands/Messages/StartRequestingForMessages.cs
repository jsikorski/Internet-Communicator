using System;
using System.Linq;
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
    public class StartRequestingForMessages : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICurrentContext _currentContext;

        public StartRequestingForMessages(
            IServerConnection serverConnection,
            IEventAggregator eventAggregator,
            ICurrentContext currentContext)
        {
            _serverConnection = serverConnection;
            _eventAggregator = eventAggregator;
            _currentContext = currentContext;
        }

        public void Execute()
        {
            var gettingMessagesTimer = new Timer(2000) { AutoReset = true };
            _currentContext.GettingMessagesTimer = gettingMessagesTimer;

            gettingMessagesTimer.Elapsed += GetMessages;
            gettingMessagesTimer.Start();
        }

        private void GetMessages(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            MessagesResponse response;

            try
            {
                response = _serverConnection.SendMessagesRequest(new MessagesRequest());
            }
            catch (Exception)
            {
                Thread.CurrentThread.Abort();
                return;
            }

            if (response.Messages.Any())
            {
                _eventAggregator.Publish(new MessagesFounded(response.Messages));
            }
        }
    }
}
