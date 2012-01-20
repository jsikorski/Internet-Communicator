using Client.Services;
using Common.Files;
using Microsoft.Win32;
using Protocol.FileTransfer;

namespace Client.Commands.Files
{
    public class DownloadFile : ICommand
    {
        private readonly File _file;
        private readonly IServerConnection _serverConnection;

        public DownloadFile(
            File file, 
            IServerConnection serverConnection)
        {
            _file = file;
            _serverConnection = serverConnection;
        }

        public void Execute()
        {

        }
    }
}
