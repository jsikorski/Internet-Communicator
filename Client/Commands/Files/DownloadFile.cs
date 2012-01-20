using Caliburn.Micro;
using Client.Messages;
using Client.Services;
using Common.Files;
using Microsoft.Win32;
using Protocol.FileTransfer;

namespace Client.Commands.Files
{
    public class DownloadFile : ICommand
    {
        private readonly FileHeader _fileHeader;
        private readonly IServerConnection _serverConnection;
        private readonly IEventAggregator _eventAggregator;

        public DownloadFile(
            FileHeader fileHeader, 
            IServerConnection serverConnection, 
            IEventAggregator eventAggregator)
        {
            _fileHeader = fileHeader;
            _serverConnection = serverConnection;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            var request = new FileDownloadRequest(_fileHeader.Guid);
            FileDownloadResponse response = _serverConnection.SendFileDownloadRequest(request);

            _eventAggregator.Publish(new FileDownloaded(response.File));
        }
    }
}
