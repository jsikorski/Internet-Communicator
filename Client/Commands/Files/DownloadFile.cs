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

        public DownloadFile(
            FileHeader fileHeader, 
            IServerConnection serverConnection)
        {
            _fileHeader = fileHeader;
            _serverConnection = serverConnection;
        }

        public void Execute()
        {

        }
    }
}
