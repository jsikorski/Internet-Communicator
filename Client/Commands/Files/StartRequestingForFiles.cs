﻿using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Autofac;
using Client.Context;
using Client.Insrastructure;
using Client.Services;
using Common.Files;
using Protocol.FileTransfer;

namespace Client.Commands.Files
{
    public class StartRequestingForFiles : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly ICurrentContext _currentContext;
        private readonly IContainer _container;

        public StartRequestingForFiles(
            IServerConnection serverConnection,
            ICurrentContext currentContext,
            IContainer container)
        {
            _serverConnection = serverConnection;
            _currentContext = currentContext;
            _container = container;
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
            FilesDownloadResponse response = _serverConnection.SendFileDownloadRequest(new FilesDownloadRequest());

            if (response.Files.Any())
            {
                ICommand command = _container.Resolve<ServiceNewFiles>(
                    new TypedParameter(typeof(IEnumerable<File>), response.Files));
                CommandInvoker.Invoke(command);
            }
        }
    }
}