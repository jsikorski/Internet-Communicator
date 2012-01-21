using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Autofac;
using Caliburn.Micro;
using Client.Context;
using Client.Insrastructure;
using Client.Messages;
using Client.Services;
using Common.Files;
using Protocol.FileTransfer;
using Timer = System.Timers.Timer;

namespace Client.Commands.Files
{
    public class StartRequestingForFiles : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly ICurrentContext _currentContext;
        private readonly IEventAggregator _eventAggregator;

        public StartRequestingForFiles(
            IServerConnection serverConnection,
            ICurrentContext currentContext,
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _currentContext = currentContext;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            var gettingFilesTimer = new Timer(2000) { AutoReset = true };
            _currentContext.GettingFilesTimer = gettingFilesTimer;

            gettingFilesTimer.Elapsed += UpdateDownloads;
            gettingFilesTimer.Start();
        }

        private void UpdateDownloads(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            FilesDownloadResponse response = null;

            try
            {
                response = _serverConnection.SendFilesDownloadRequest(new FilesDownloadRequest());
            }
            catch (Exception)
            {
                Thread.CurrentThread.Abort();
                return;
            }

            if (response.FileHeaders.Any())
            {
                _eventAggregator.Publish(new FilesFounded(response.FileHeaders));
            }
        }
    }
}
